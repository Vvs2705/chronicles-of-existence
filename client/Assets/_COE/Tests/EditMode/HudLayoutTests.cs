using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace COE.Tests
{
    /// <summary>Bloco D (ADR-0006): a HUD de jogo em paisagem nas proporcoes de celular 16:9, 19.5:9 e 20:9 (1080 px de
    /// altura, ~400 dpi) e numa tela baixa (720 px a 320 dpi = 360 dp), com e sem notch, nas duas maos. Tudo na area segura;
    /// nada cobre os controles de toque nem outro elemento que aparece junto; cartao e prompt com espaco para ler.</summary>
    public class HudLayoutTests
    {
        struct Caso
        {
            public string Nome; public Vector2 Tela; public float Dpi;
            public Caso(string nome, float w, float h, float dpi) { Nome = nome; Tela = new Vector2(w, h); Dpi = dpi; }
        }

        static readonly Caso[] Telas =
        {
            new Caso("16:9", 1920, 1080, 400), new Caso("19.5:9", 2340, 1080, 400), new Caso("20:9 POCO F4", 2400, 1080, 395),
            new Caso("16:9 baixa 360dp", 1280, 720, 320), new Caso("20:9 baixa 360dp", 1600, 720, 320),
            new Caso("20:9 360dp a 480 dpi", 2400, 1080, 480),
        };

        /// <summary>Prompt Mestre §9 (legibilidade em tela pequena): cartao da missao, prompt, dano e conversa com pelo menos
        /// MinTextoDp (12 sp) em toda tela. O piso antigo era em px: a 480 dpi o cartao dava 9 dp.</summary>
        [Test]
        public void TextoDaHud_TemPeloMenosOMinimoEmDp_EmTodaTela()
        {
            foreach (Caso c in Telas)
            {
                float minimo = ControlPreset.DpToPx(HudLayout.MinTextoDp, c.Dpi) - 0.5f;
                Assert.GreaterOrEqual(HudLayout.FonteCartao(c.Tela.y, c.Dpi), minimo, c.Nome + ": cartao da missao");
                Assert.GreaterOrEqual(HudLayout.FontePrompt(c.Tela.y, c.Dpi), minimo, c.Nome + ": prompt, dano e conversa");
            }
        }

        static IEnumerable<KeyValuePair<string, Rect>> AreasSeguras(Vector2 t)
        {
            yield return new KeyValuePair<string, Rect>("sem notch", new Rect(0, 0, t.x, t.y));
            yield return new KeyValuePair<string, Rect>("notch a esquerda", new Rect(100, 0, t.x - 100, t.y));
            yield return new KeyValuePair<string, Rect>("notch a direita", new Rect(0, 0, t.x - 100, t.y));
            yield return new KeyValuePair<string, Rect>("notch e barra", new Rect(90, 40, t.x - 180, t.y - 40));
        }

        [Test]
        public void HudDeJogo_NaAreaSegura_SemCobrirOToque_EmTodaProporcao_NasDuasMaos()
        {
            foreach (Caso c in Telas)
                foreach (var area in AreasSeguras(c.Tela))
                    foreach (HandPreset mao in new[] { HandPreset.Destro, HandPreset.Canhoto })
                    {
                        ControlPreset p = ControlPreset.Default(mao);
                        try { Conferir(c, area.Key, area.Value, p); }
                        finally { Object.DestroyImmediate(p); }
                    }
        }

        static void Conferir(Caso c, string nomeArea, Rect safe, ControlPreset p)
        {
            string caso = c.Nome + " / " + nomeArea + " / " + p.hand;
            float alvo = HudLayout.Alvo(c.Tela.y, c.Dpi);
            int fonteCartao = HudLayout.FonteCartao(c.Tela.y, c.Dpi), fontePrompt = HudLayout.FontePrompt(c.Tela.y, c.Dpi);

            List<Rect> toque = HudLayout.Toque(p, safe, c.Dpi);
            Rect menu = HudLayout.BotaoMenu(safe, alvo);
            Rect cartao = HudLayout.CartaoMissao(safe, fonteCartao, p, c.Dpi);
            Rect prompt = HudLayout.Prompt(safe, c.Dpi, p, fontePrompt);
            Rect seguir = HudLayout.BotaoSeguir(c.Tela, safe, alvo);
            Rect treino = HudLayout.LinhaTreino(safe);
            Rect aviso = HudLayout.FaixaDeAviso(safe, c.Tela.y, fontePrompt);
            Rect barras = HudLayout.Barras(safe, c.Tela.y, alvo, fonteCartao, fontePrompt);

            var sempre = new Dictionary<string, Rect> { { "menu", menu }, { "cartao", cartao }, { "prompt", prompt } };
            for (int i = 0; i < toque.Count; i++) sempre.Add(i < p.buttons.Length ? "toque." + p.buttons[i].action : "joystick", toque[i]);

            foreach (var e in sempre) DentroDaArea(caso, e.Key, e.Value, safe);
            DentroDaArea(caso, "seguir", seguir, safe);
            DentroDaArea(caso, "treino", treino, safe);

            SemSobreposicao(caso, sempre, null, default(Rect));
            SemSobreposicao(caso, sempre, "seguir (aos 5)", seguir);
            SemSobreposicao(caso, sempre, "treino (aos 8)", treino);
            DentroDaArea(caso, "barras", barras, safe);
            SemSobreposicao(caso, sempre, "barras de vida/vigor/mana (aos 8)", barras);
            Assert.IsFalse(barras.Overlaps(treino), caso + ": barras cobrem o painel do treino");
            Assert.IsFalse(barras.Overlaps(aviso), caso + ": barras cobrem o aviso da conversa");
            Assert.GreaterOrEqual(barras.width, fonteCartao * 9f, caso + ": barras sem largura para os tres nomes");

            Assert.GreaterOrEqual(menu.height, ControlPreset.DpToPx(ControlPreset.MinTargetDp, c.Dpi) - 0.5f, caso + ": menu abaixo de 48 dp");
            DentroDaArea(caso, "painel do menu de pausa", HudLayout.PainelDoMenu(safe, alvo, HudLayout.Fonte(14f, 1f / 40f, c.Tela.y, c.Dpi)), safe);
            Assert.GreaterOrEqual(seguir.height, ControlPreset.DpToPx(ControlPreset.MinTargetDp, c.Dpi) - 0.5f, caso + ": seguir abaixo de 48 dp");
            Assert.GreaterOrEqual(cartao.height, fonteCartao * 2.5f, caso + ": cartao de missao sem altura para duas linhas");
            Assert.GreaterOrEqual(prompt.width, fontePrompt * 4f, caso + ": prompt sem largura para um nome (texto maior encolhe)");
        }

        static void DentroDaArea(string caso, string nome, Rect r, Rect safe)
        {
            Assert.IsTrue(r.xMin >= safe.xMin - 0.5f && r.xMax <= safe.xMax + 0.5f && r.yMin >= safe.yMin - 0.5f && r.yMax <= safe.yMax + 0.5f,
                caso + ": " + nome + " fora da area segura " + r + " / " + safe);
        }

        /// <summary>Cada par de "sempre" (toque contra toque fica com o TouchControlsTests), ou um extra contra todos eles.</summary>
        static void SemSobreposicao(string caso, Dictionary<string, Rect> sempre, string extra, Rect r)
        {
            var nomes = new List<string>(sempre.Keys);
            for (int i = 0; i < nomes.Count; i++)
            {
                if (extra != null)
                {
                    Assert.IsFalse(r.Overlaps(sempre[nomes[i]]), caso + ": " + extra + " cobre " + nomes[i]);
                    continue;
                }
                for (int j = i + 1; j < nomes.Count; j++)
                {
                    bool doisDeToque = (nomes[i].StartsWith("toque.") || nomes[i] == "joystick") && (nomes[j].StartsWith("toque.") || nomes[j] == "joystick");
                    if (doisDeToque) continue;
                    Assert.IsFalse(sempre[nomes[i]].Overlaps(sempre[nomes[j]]), caso + ": " + nomes[i] + " cobre " + nomes[j]
                        + " (" + sempre[nomes[i]] + " x " + sempre[nomes[j]] + ")");
                }
            }
        }
    }
}
