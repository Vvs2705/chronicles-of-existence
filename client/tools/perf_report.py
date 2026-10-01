"""Resumo de um CSV do PerfHud (client/Assets/_COE/Scripts/Perf/PerfHud.cs).

Uso:
  python client/tools/perf_report.py <perf_*.csv> [--meta-fps 30] [--aquecimento 5] [--md saida.md]
  python client/tools/perf_report.py --autoteste

Contrato do CSV (o PerfHud escreve, este script confere):
  - as 8 primeiras colunas, nesta ordem: t_s,fps,frame_ms,alloc_mb,battery,temp_c,device,gpu
  - fps_min_1s (9a) e colunas extras sao lidas por NOME; extras sao toleradas
  - "n/d" (ou vazio) = dado ausente
  - periodo de amostragem = mediana dos deltas de t_s

O aquecimento (segundos iniciais, carga da cena) fica fora do resumo. Sai com codigo 2 se o CSV nao segue o contrato.
So biblioteca padrao: roda em qualquer Python 3 sem instalar nada.
"""
import argparse
import csv
import io
import statistics
import sys

CABECALHO = ["t_s", "fps", "frame_ms", "alloc_mb", "battery", "temp_c", "device", "gpu"]
ENGASGO_FRACAO = 0.5   # fps_min_1s abaixo de metade da meta = um quadro que o jogador sente


class CsvInvalido(Exception):
    pass


def _num(v):
    v = (v or "").strip()
    if v in ("", "n/d"):
        return None
    return float(v)


def ler(texto):
    linhas = list(csv.reader(io.StringIO(texto)))
    if not linhas:
        raise CsvInvalido("arquivo vazio")
    cab = [c.strip() for c in linhas[0]]
    if cab[:8] != CABECALHO:
        raise CsvInvalido("cabecalho fora do contrato: %s (esperado %s...)" % (",".join(cab[:8]), ",".join(CABECALHO)))
    amostras = []
    for n, l in enumerate(linhas[1:], start=2):
        if not l or all(not c.strip() for c in l):
            continue
        if len(l) < len(cab):
            raise CsvInvalido("linha %d com %d colunas, cabecalho tem %d" % (n, len(l), len(cab)))
        amostras.append(dict(zip(cab, l)))
    if not amostras:
        raise CsvInvalido("sem amostras")
    return cab, amostras


def percentil(valores, p):
    """Percentil por posicao mais proxima (p em 0..100). Lista vazia = None."""
    if not valores:
        return None
    v = sorted(valores)
    k = max(0, min(len(v) - 1, int(round(p / 100.0 * (len(v) - 1)))))
    return v[k]


def resumir(cab, amostras, meta_fps=30.0, aquecimento=5.0):
    t = [_num(a["t_s"]) for a in amostras]
    inicio = t[0]
    uteis = [a for a, ti in zip(amostras, t) if ti is not None and ti - inicio >= aquecimento]
    if not uteis:
        raise CsvInvalido("nenhuma amostra depois de %.0f s de aquecimento" % aquecimento)
    deltas = [b - a for a, b in zip(t, t[1:]) if a is not None and b is not None]
    col = lambda nome: [x for x in (_num(a.get(nome)) for a in uteis) if x is not None]

    fps, frame, mem = col("fps"), col("frame_ms"), col("alloc_mb")
    fps_min = col("fps_min_1s") if "fps_min_1s" in cab else []
    temp, bat = col("temp_c"), col("battery")
    r = {
        "aparelho": uteis[0]["device"], "gpu": uteis[0]["gpu"],
        "amostras": len(uteis), "descartadas_aquecimento": len(amostras) - len(uteis),
        "duracao_s": _num(uteis[-1]["t_s"]) - _num(uteis[0]["t_s"]),
        "periodo_s": statistics.median(deltas) if deltas else None,
        "meta_fps": meta_fps,
        "fps_mediana": statistics.median(fps) if fps else None,
        "fps_p5": percentil(fps, 5),
        "fps_min": min(fps) if fps else None,
        "fps_na_meta_pct": 100.0 * sum(1 for f in fps if f >= 0.95 * meta_fps) / len(fps) if fps else None,
        "engasgos": sum(1 for f in fps_min if f < ENGASGO_FRACAO * meta_fps),
        "pior_quadro_fps": min(fps_min) if fps_min else None,
        "frame_ms_mediana": statistics.median(frame) if frame else None,
        "frame_ms_p95": percentil(frame, 95),
        "mem_mb_pico": max(mem) if mem else None,
        "mem_mb_crescimento": (mem[-1] - mem[0]) if mem else None,
        "temp_c_inicio": temp[0] if temp else None, "temp_c_max": max(temp) if temp else None,
        "temp_c_fim": temp[-1] if temp else None,
        "bateria_inicio": bat[0] if bat else None, "bateria_fim": bat[-1] if bat else None,
        "extras": [c for c in cab if c not in CABECALHO + ["fps_min_1s"]],
    }
    return r


def _f(v, fmt="%.1f", nd="n/d"):
    return nd if v is None else fmt % v


def markdown(r, nome):
    linhas = [
        "# Medição: %s" % nome,
        "",
        "- **Aparelho:** %s · %s" % (r["aparelho"], r["gpu"]),
        "- **Amostras:** %d úteis em %s s (período %s s); %d de aquecimento descartadas" % (
            r["amostras"], _f(r["duracao_s"], "%.0f"), _f(r["periodo_s"], "%.2f"), r["descartadas_aquecimento"]),
        "",
        "| Métrica | Valor |",
        "|---|---|",
        "| FPS mediana / p5 / mínimo (suavizado) | %s / %s / %s |" % (_f(r["fps_mediana"]), _f(r["fps_p5"]), _f(r["fps_min"])),
        "| Amostras na meta (≥ 95%% de %s FPS) | %s%% |" % (_f(r["meta_fps"], "%.0f"), _f(r["fps_na_meta_pct"], "%.0f")),
        "| Engasgos (pior quadro < %s FPS) / pior quadro | %d / %s FPS |" % (
            _f(ENGASGO_FRACAO * r["meta_fps"], "%.0f"), r["engasgos"], _f(r["pior_quadro_fps"])),
        "| Tempo de quadro mediana / p95 | %s / %s ms |" % (_f(r["frame_ms_mediana"]), _f(r["frame_ms_p95"])),
        "| Memória alocada pico / crescimento | %s / %s MB |" % (_f(r["mem_mb_pico"], "%.0f"), _f(r["mem_mb_crescimento"], "%+.0f")),
        "| Temperatura início / máx / fim | %s / %s / %s °C |" % (_f(r["temp_c_inicio"]), _f(r["temp_c_max"]), _f(r["temp_c_fim"])),
        "| Bateria início / fim | %s / %s |" % (_f(r["bateria_inicio"], "%.2f"), _f(r["bateria_fim"], "%.2f")),
    ]
    if r["extras"]:
        linhas.append("")
        linhas.append("Colunas extras (contadores de jogo): " + ", ".join(r["extras"]))
    return "\n".join(linhas) + "\n"


def _autoteste():
    base = "t_s,fps,frame_ms,alloc_mb,battery,temp_c,device,gpu,fps_min_1s,inimigos\n"
    corpo = "".join("%d,%s,33.3,%d,0.90,n/d,Fone X,Adreno,%s,2\n" % (
        i, "3.0" if i < 3 else "30.0", 90 + i // 10, "2.0" if i == 12 else "29.5") for i in range(30))
    cab, a = ler(base + corpo)
    r = resumir(cab, a, meta_fps=30, aquecimento=5)
    assert r["amostras"] == 25 and r["descartadas_aquecimento"] == 5, r
    assert r["periodo_s"] == 1.0, r["periodo_s"]
    assert r["fps_min"] == 30.0, "o aquecimento (3 FPS da carga) fica fora do resumo"
    assert r["engasgos"] == 1 and r["pior_quadro_fps"] == 2.0, "o quadro de 2 FPS aparece mesmo com o fps suavizado em 30"
    assert r["temp_c_max"] is None, "n/d = ausente"
    assert r["mem_mb_crescimento"] == 2, r["mem_mb_crescimento"]
    assert r["extras"] == ["inimigos"], "coluna extra tolerada e reportada"
    for ruim in ("fps,t_s\n1,2\n", "", base):
        try:
            resumir(*ler(ruim))
            raise AssertionError("aceitou CSV fora do contrato: %r" % ruim[:20])
        except CsvInvalido:
            pass
    assert "| FPS mediana" in markdown(r, "x")
    print("autoteste ok")


def main(argv=None):
    if hasattr(sys.stdout, "reconfigure"):
        sys.stdout.reconfigure(encoding="utf-8")   # console do Windows (cp1252) nao tem "≥" nem "°"
    p = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    p.add_argument("csv", nargs="?")
    p.add_argument("--meta-fps", type=float, default=30.0)
    p.add_argument("--aquecimento", type=float, default=5.0, help="segundos iniciais fora do resumo (carga da cena)")
    p.add_argument("--md", help="grava o relatorio em Markdown neste arquivo")
    p.add_argument("--autoteste", action="store_true")
    a = p.parse_args(argv)
    if a.autoteste:
        _autoteste()
        return 0
    if not a.csv:
        p.error("informe o CSV")
    try:
        with open(a.csv, encoding="utf-8") as f:
            cab, amostras = ler(f.read())
        r = resumir(cab, amostras, a.meta_fps, a.aquecimento)
    except (CsvInvalido, ValueError) as e:
        print("CSV invalido: %s" % e, file=sys.stderr)
        return 2
    md = markdown(r, a.csv.replace("\\", "/").split("/")[-1])
    if a.md:
        with open(a.md, "w", encoding="utf-8") as f:
            f.write(md)
    sys.stdout.write(md)
    return 0


if __name__ == "__main__":
    sys.exit(main())
