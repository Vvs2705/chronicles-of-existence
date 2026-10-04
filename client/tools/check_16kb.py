#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Confere se as bibliotecas nativas de um AAB/APK (ou um .so solto) estao prontas para paginas de 16 KB
(exigencia do Google Play para apps com target 35+ a partir de 01/11/2025; prazo do COE em docs/PROJETO.md §6).

Regra: todo segmento PT_LOAD de cada .so ARM64 precisa de p_align >= 16384. So biblioteca padrao (zipfile + struct).
Uso:
  python client/tools/check_16kb.py client/Builds/android/COE_0.1.0_123.aab
  python client/tools/check_16kb.py --autoteste
Saida 0 = tudo alinhado; 1 = alguma biblioteca com alinhamento menor; 2 = nada para conferir / arquivo invalido.
Nao confere o alinhamento do ZIP do APK (zipalign -P 16): para AAB quem gera os APKs e a Play.
"""

import io
import struct
import sys
import zipfile

PT_LOAD = 1
PAGINA = 16384


def alinhamentos(elf):
    """Lista (p_align) de cada PT_LOAD de um ELF de 64 bits little-endian. ValueError se nao for ELF64 LE."""
    if len(elf) < 64 or elf[:4] != b"\x7fELF":
        raise ValueError("nao e ELF")
    if elf[4] != 2 or elf[5] != 1:
        raise ValueError("so ELF64 little-endian (arm64-v8a)")
    e_phoff, = struct.unpack_from("<Q", elf, 0x20)
    e_phentsize, e_phnum = struct.unpack_from("<HH", elf, 0x36)
    r = []
    for i in range(e_phnum):
        off = e_phoff + i * e_phentsize
        p_type, = struct.unpack_from("<I", elf, off)
        if p_type == PT_LOAD:
            p_align, = struct.unpack_from("<Q", elf, off + 48)
            r.append(p_align)
    return r


def conferir_bytes(nome, elf):
    """(ok, texto) de um .so."""
    try:
        al = alinhamentos(elf)
    except ValueError as e:
        return None, "%s: ignorado (%s)" % (nome, e)
    ruins = [a for a in al if a < PAGINA]
    if not al:
        return False, "%s: sem segmento PT_LOAD" % nome
    if ruins:
        return False, "%s: PT_LOAD com alinhamento %s (< 16 KB)" % (nome, ", ".join(hex(a) for a in sorted(set(ruins))))
    return True, "%s: ok (%s)" % (nome, ", ".join(hex(a) for a in sorted(set(al))))


def conferir(caminho):
    linhas, oks = [], []
    if caminho.lower().endswith(".so"):
        with open(caminho, "rb") as f:
            ok, t = conferir_bytes(caminho, f.read())
        linhas.append(t)
        if ok is not None:
            oks.append(ok)
    else:
        try:
            z = zipfile.ZipFile(caminho)
        except (OSError, zipfile.BadZipFile) as e:
            print("arquivo invalido: %s (%s)" % (caminho, e))
            return 2
        with z:
            for n in z.namelist():
                if n.endswith(".so") and "/arm64-v8a/" in "/" + n:
                    ok, t = conferir_bytes(n, z.read(n))
                    linhas.append(t)
                    if ok is not None:
                        oks.append(ok)
    for t in linhas:
        print("  " + t)
    if not oks:
        print("nenhuma biblioteca arm64-v8a para conferir em %s" % caminho)
        return 2
    if all(oks):
        print("OK: %d bibliotecas arm64 alinhadas a 16 KB" % len(oks))
        return 0
    print("FALHOU: %d de %d bibliotecas sem alinhamento de 16 KB" % (oks.count(False), len(oks)))
    return 1


def _elf_falso(aligns):
    """ELF64 LE minimo com um PT_LOAD por alinhamento (so cabecalhos: o suficiente para o leitor)."""
    phoff, phentsize = 64, 56
    cab = bytearray(64)
    cab[:4] = b"\x7fELF"
    cab[4], cab[5], cab[6] = 2, 1, 1
    struct.pack_into("<Q", cab, 0x20, phoff)
    struct.pack_into("<HH", cab, 0x36, phentsize, len(aligns))
    ph = bytearray()
    for a in aligns:
        e = bytearray(phentsize)
        struct.pack_into("<I", e, 0, PT_LOAD)
        struct.pack_into("<Q", e, 48, a)
        ph += e
    return bytes(cab + ph)


def autoteste():
    assert alinhamentos(_elf_falso([0x4000, 0x4000])) == [0x4000, 0x4000]
    assert conferir_bytes("bom.so", _elf_falso([0x4000, 0x10000]))[0] is True
    assert conferir_bytes("ruim.so", _elf_falso([0x4000, 0x1000]))[0] is False
    assert conferir_bytes("vazio.so", _elf_falso([]))[0] is False
    assert conferir_bytes("texto.so", b"nao e elf")[0] is None
    buf = io.BytesIO()
    with zipfile.ZipFile(buf, "w") as z:
        z.writestr("base/lib/arm64-v8a/libbom.so", _elf_falso([0x4000]))
        z.writestr("base/lib/arm64-v8a/libruim.so", _elf_falso([0x1000]))
        z.writestr("base/lib/armeabi-v7a/libignorado.so", _elf_falso([0x1000]))
    import os
    import tempfile
    caminho = os.path.join(tempfile.mkdtemp(), "teste.aab")
    with open(caminho, "wb") as f:
        f.write(buf.getvalue())
    assert conferir(caminho) == 1, "a biblioteca ruim tem de reprovar o pacote"
    print("autoteste ok")
    return 0


if __name__ == "__main__":
    if len(sys.argv) == 2 and sys.argv[1] == "--autoteste":
        sys.exit(autoteste())
    if len(sys.argv) != 2:
        print(__doc__)
        sys.exit(2)
    sys.exit(conferir(sys.argv[1]))
