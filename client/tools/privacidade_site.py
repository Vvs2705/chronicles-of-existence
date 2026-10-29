"""privacidade_site.py - a politica de privacidade em paginas HTML (4 idiomas) para o GitHub Pages, a partir de
docs/privacidade/POLITICA.md (o texto-fonte; a pagina nunca e editada a mao).

Uso (da raiz do repositorio):
  python -I client/tools/privacidade_site.py --data 2026-11-25 [--saida client/Builds/privacidade]
        gera index.html (escolhe o idioma pelo navegador, com links para os 4), pt-BR.html, en.html, es.html, id.html
  python -I client/tools/privacidade_site.py --autoteste
        gera numa pasta temporaria e confere: 4 idiomas, 8 itens em cada, contato, sem placeholder sobrando

Publicar NAO e daqui: e o workflow .github/workflows/privacidade-pages.yml, que so roda a mao (workflow_dispatch) e so com o
aval do idealizador (publicacao externa; docs/LOJA.md sec. 4). URL esperada: https://vvs2705.github.io/chronicles-of-existence/
(a da ficha da Play aponta para o index.html).
"""
import argparse, html, os, re, sys, tempfile

RAIZ = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
FONTE = os.path.join(RAIZ, "docs", "privacidade", "POLITICA.md")
SAIDA = os.path.join(RAIZ, "client", "Builds", "privacidade")
# titulo da secao no POLITICA.md -> (codigo, lang do HTML, titulo da pagina, rotulo do link)
IDIOMAS = {
    "Português (Brasil)": ("pt-BR", "pt-BR", "Política de privacidade", "Português"),
    "English": ("en", "en", "Privacy policy", "English"),
    "Español": ("es", "es", "Política de privacidad", "Español"),
    "Bahasa Indonesia": ("id", "id", "Kebijakan privasi", "Bahasa Indonesia"),
}
PLACEHOLDER = re.compile(r"\[(data da publicação|publication date|fecha de publicación|tanggal publikasi)\]")
JOGO = "Chronicles of Existence"

CSS = """
:root{--fundo:#F6F1E6;--tinta:#1B2433;--fio:#B8963F;--azul:#253850}
@media (prefers-color-scheme:dark){:root{--fundo:#121B27;--tinta:#E9DEC6;--fio:#D6B36A;--azul:#86C8C9}}
*{box-sizing:border-box}body{margin:0;background:var(--fundo);color:var(--tinta);font:17px/1.6 system-ui,-apple-system,"Segoe UI",Roboto,sans-serif}
main{max-width:720px;margin:0 auto;padding:32px 16px 64px}h1{font-family:Georgia,serif;font-size:1.7em;margin:0 0 4px;color:var(--azul)}
.jogo{letter-spacing:.08em;text-transform:uppercase;font-size:.8em;opacity:.75;margin:0}hr{border:0;border-top:2px solid var(--fio);margin:16px 0 24px}
nav{display:flex;flex-wrap:wrap;gap:8px 16px;font-size:.9em;margin:0 0 8px}a{color:var(--azul)}ol{padding-left:1.3em}li{margin:0 0 12px}
.data{font-size:.9em;opacity:.8}footer{margin-top:32px;font-size:.85em;opacity:.8}
"""


def secoes(md):
    """{titulo da secao: corpo em markdown}, so as secoes de idioma do POLITICA.md."""
    out, atual, linhas = {}, None, []
    for l in md.splitlines():
        m = re.match(r"^## (.+)$", l)
        if m:
            if atual:
                out[atual] = "\n".join(linhas)
            atual, linhas = (m.group(1).strip() if m.group(1).strip() in IDIOMAS else None), []
        elif atual:
            linhas.append(l)
    if atual:
        out[atual] = "\n".join(linhas)
    return out


def inline(t):
    t = html.escape(t, quote=False)
    t = re.sub(r"\*\*(.+?)\*\*", r"<strong>\1</strong>", t)
    t = re.sub(r"(https://[^\s<)]+[^\s<).,])", r'<a href="\1">\1</a>', t)
    t = re.sub(r"\b([\w.+-]+@[\w-]+\.[\w.]+[\w])\b", r'<a href="mailto:\1">\1</a>', t)
    return t


def corpo_html(md):
    partes, lista = [], []
    for l in md.strip().splitlines():
        l = l.strip()
        if not l or l == "---":
            continue
        m = re.match(r"^\d+\.\s+(.*)$", l)
        if m:
            lista.append("<li>%s</li>" % inline(m.group(1)))
            continue
        if lista:
            partes.append("<ol>%s</ol>" % "".join(lista))
            lista = []
        cls = ' class="data"' if l.startswith("**") and PLACEHOLDER.search(l) is None and re.search(r"\d{4}-\d{2}-\d{2}", l) else ""
        partes.append("<p%s>%s</p>" % (cls, inline(l)))
    if lista:
        partes.append("<ol>%s</ol>" % "".join(lista))
    return "\n".join(partes)


def nav(atual):
    return "<nav>" + " · ".join(
        ('<strong>%s</strong>' % rot) if cod == atual else '<a href="%s.html" hreflang="%s">%s</a>' % (cod, lang, rot)
        for cod, lang, _, rot in IDIOMAS.values()) + "</nav>"


def pagina(lang, titulo, conteudo, atual):
    return ('<!doctype html>\n<html lang="%s"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1">'
            '<title>%s · %s</title><style>%s</style></head><body><main>%s<p class="jogo">%s</p><h1>%s</h1><hr>%s'
            '<footer>V-STACK Solutions</footer></main></body></html>\n') % (lang, titulo, JOGO, CSS, nav(atual), JOGO, titulo, conteudo)


def gerar(data, saida, fonte=FONTE):
    with open(fonte, encoding="utf-8") as f:
        sec = secoes(f.read())
    faltam = [t for t in IDIOMAS if t not in sec]
    if faltam:
        raise SystemExit("POLITICA.md sem a secao: " + ", ".join(faltam))
    os.makedirs(saida, exist_ok=True)
    for titulo_sec, (cod, lang, titulo, _) in IDIOMAS.items():
        md = PLACEHOLDER.sub(data, sec[titulo_sec])
        with open(os.path.join(saida, cod + ".html"), "w", encoding="utf-8") as f:
            f.write(pagina(lang, titulo, corpo_html(md), cod))
    # index: escolhe pelo idioma do navegador (sem script, mostra os 4 links); fora dos 4, ingles (como o jogo)
    links = "".join('<li><a href="%s.html" hreflang="%s">%s</a></li>' % (c, l, r) for c, l, _, r in IDIOMAS.values())
    js = ("<script>var l=(navigator.language||'en').toLowerCase(),d=l.indexOf('pt')==0?'pt-BR':l.indexOf('es')==0?'es':"
          "(l.indexOf('id')==0||l.indexOf('in')==0)?'id':'en';location.replace(d+'.html');</script>")
    with open(os.path.join(saida, "index.html"), "w", encoding="utf-8") as f:
        f.write(pagina("en", "Privacy policy", "<ol>%s</ol>%s" % (links, js), None))
    return [os.path.join(saida, n) for n in ["index.html"] + [v[0] + ".html" for v in IDIOMAS.values()]]


def autoteste():
    erros = []
    with tempfile.TemporaryDirectory() as t:
        arqs = gerar("2026-11-25", t)
        for a in arqs:
            txt = open(a, encoding="utf-8").read()
            nome = os.path.basename(a)
            if nome == "index.html":
                if txt.count("<li>") != 4:
                    erros.append("index sem os 4 idiomas")
                continue
            if txt.count("<li>") != 8:
                erros.append("%s: %d itens (esperado 8)" % (nome, txt.count("<li>")))
            if "2026-11-25" not in txt or PLACEHOLDER.search(txt):
                erros.append(nome + ": data nao preenchida")
            if 'href="mailto:vinicius@vstack-solutions.com.br"' not in txt:
                erros.append(nome + ": sem o contato")
            if "https://unity.com/legal/privacy-policy" not in txt:
                erros.append(nome + ": sem o link da politica da Unity")
            if "**" in txt or "<script" in txt:
                erros.append(nome + ": markdown sobrando ou script na pagina")
    for e in erros:
        print("FALHOU", e)
    print("privacidade_site --autoteste:", "FALHOU" if erros else "OK")
    return 1 if erros else 0


if __name__ == "__main__":
    ap = argparse.ArgumentParser()
    ap.add_argument("--data", help="data da publicacao (AAAA-MM-DD), vai no 'Ultima atualizacao' das 4 paginas")
    ap.add_argument("--saida", default=SAIDA)
    ap.add_argument("--autoteste", action="store_true")
    a = ap.parse_args()
    if a.autoteste:
        sys.exit(autoteste())
    if not a.data or not re.match(r"^\d{4}-\d{2}-\d{2}$", a.data):
        sys.exit("--data AAAA-MM-DD obrigatorio (a data que aparece na politica publicada)")
    for p in gerar(a.data, a.saida):
        print("OK", os.path.relpath(p))
