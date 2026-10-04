using System.Collections.Generic;
using UnityEngine;

namespace COE
{
    /// <summary>Geometria das HUDs de jogo (fora dos modais), em px de tela com origem embaixo a esquerda. C# puro: as HUDs
    /// desenham por aqui e o HudLayoutTests confere em 16:9, 19.5:9, 20:9 e numa tela baixa (360 dp), nas duas maos, com e
    /// sem notch, que tudo fica na area segura e nada cobre os controles de toque (Bloco D, ADR-0006).
    /// Modais (conversa, menu, salto, gancho, saida, entrada) escondem o toque (UiFundo) e nao entram aqui.</summary>
    public static class HudLayout
    {
        /// <summary>Alvo de toque: 48 dp no minimo e ~1/10 da altura em tela grande.</summary>
        public static float Alvo(float alturaTela, float dpi)
        {
            return Mathf.Max(ControlPreset.DpToPx(ControlPreset.MinTargetDp, dpi), alturaTela / 10f);
        }

        /// <summary>Corpo de texto: `dp` no minimo e uma fracao da altura (1/40 = texto corrido, 1/30 = destaque).</summary>
        public static int Fonte(float dp, float fracaoDaAltura, float alturaTela, float dpi)
        {
            return Mathf.RoundToInt(Mathf.Max(ControlPreset.DpToPx(dp, dpi), alturaTela * fracaoDaAltura));
        }

        /// <summary>Linhas do painel do menu de pausa: mao|som, sensibilidade|fps, desempenho, qualidade, voltar.</summary>
        public const int LinhasDoMenu = 5;

        /// <summary>Painel do menu de pausa (modal: o toque some). Grade de 2 colunas com alvos de linha inteira: 5 linhas cabem
        /// ate num celular de 360 dp com barra; a coluna unica de 7 linhas de 48 dp nao cabia (o painel saia da tela).</summary>
        public static Rect PainelDoMenu(Rect safe, float alvo, int fonte)
        {
            float m = alvo * 0.25f, gap = alvo * 0.15f, tituloH = fonte * 1.3f * 1.6f;
            float h = 2f * m + tituloH + gap + LinhasDoMenu * alvo + (LinhasDoMenu - 1) * gap;
            float w = safe.width * 0.8f;
            return new Rect(safe.x + (safe.width - w) * 0.5f, safe.y + Mathf.Max(0f, (safe.height - h) * 0.5f), w, h);
        }

        /// <summary>Fonte do cartao de missao: proporcional a tela (1/40), piso de MinTextoDp.</summary>
        public static int FonteCartao(float alturaTela, float dpi) { return Fonte(MinTextoDp, 1f / 40f, alturaTela, dpi); }

        /// <summary>Fonte do prompt de interacao, dos numeros de dano e da conversa: 1/30 da altura, piso de MinTextoDp.</summary>
        public static int FontePrompt(float alturaTela, float dpi) { return Fonte(MinTextoDp, 1f / 30f, alturaTela, dpi); }

        /// <summary>Menor texto de leitura na tela do jogo, em dp (12 sp: o minimo de legenda do Android). O piso era em px
        /// (14 e 18): a 480 dpi, o cartao da missao saia com 9 dp. ponytail: sem escala de fonte do sistema; ela entra com a
        /// opcao de tamanho de texto no menu, se o playtest pedir.</summary>
        public const float MinTextoDp = 12f;

        /// <summary>O que os controles de toque desenham: o quadrado de cada botao e o do joystick em repouso.</summary>
        public static List<Rect> Toque(ControlPreset p, Rect safe, float dpi)
        {
            var r = new List<Rect>(p.buttons.Length + 1);
            for (int i = 0; i < p.buttons.Length; i++) r.Add(Quadrado(p.ButtonCenterPx(i, safe, dpi), p.ButtonRadiusPx(i, dpi)));
            r.Add(Quadrado(p.JoystickRestPx(safe, dpi), ControlPreset.DpToPx(p.joystickRadiusDp, dpi)));
            return r;
        }

        /// <summary>Engrenagem do menu: no topo, logo a esquerda da coluna do cartao de missao (80%+ da area segura). Pela
        /// area segura, como os botoes de toque: com notch a direita, pela tela ela caia em cima do USAR (HudLayoutTests).</summary>
        public static Rect BotaoMenu(Rect safe, float alvo)
        {
            float m = alvo * 0.25f;
            return new Rect(safe.x + safe.width * 0.8f - m - alvo, safe.yMax - m - alvo, alvo, alvo);
        }

        /// <summary>Area MAXIMA do cartao de missao: coluna da direita (80%+ da area segura), do topo ate acima do controle de
        /// toque mais alto daquela coluna (botoes no destro, joystick no canhoto). Em tela baixa (360 dp, barra embaixo) os
        /// botoes do destro tomam a coluna quase inteira: sem ~3 linhas livres, o cartao vai para a coluna da esquerda, acima
        /// do joystick, onde ha o dobro de altura. O cartao usa a parte de cima e encolhe a fonte se o texto nao couber.</summary>
        public static Rect CartaoMissao(Rect safe, int fonte, ControlPreset p, float dpi)
        {
            float margem = fonte * 0.6f;
            Rect direita = Coluna(safe.x + safe.width * 0.8f, safe.xMax - margem, safe, margem, p, dpi);
            if (direita.height >= fonte * 4f) return direita;
            Rect esquerda = Coluna(safe.x + margem, safe.x + safe.width * 0.2f, safe, margem, p, dpi);
            return esquerda.height > direita.height ? esquerda : direita;
        }

        static Rect Coluna(float x0, float x1, Rect safe, float margem, ControlPreset p, float dpi)
        {
            float baixo = safe.y + margem;
            foreach (Rect t in Toque(p, safe, dpi))
                if (t.xMax > x0 && t.xMin < x1) baixo = Mathf.Max(baixo, t.yMax + margem);
            float topo = safe.yMax - margem;
            return new Rect(x0, baixo, x1 - x0, Mathf.Max(0f, topo - baixo));
        }

        /// <summary>"Seguir adiante" do salto (so aos 5, fora da clareira): topo ao centro, abaixo da faixa de aviso.</summary>
        public static Rect BotaoSeguir(Vector2 tela, Rect safe, float alvo)
        {
            return new Rect(safe.x + safe.width * 0.35f, safe.yMax - tela.y * 0.1f - alvo, safe.width * 0.3f, alvo);
        }

        /// <summary>Faixa do aviso da conversa (DialogueHud: "esta ocupado" e afins): no topo, 20%-80% da area segura.</summary>
        public static Rect FaixaDeAviso(Rect safe, float alturaTela, int fonte)
        {
            return new Rect(safe.x + safe.width * 0.2f, safe.yMax - alturaTela * 0.01f - fonte * 1.8f, safe.width * 0.6f, fonte * 1.8f);
        }

        /// <summary>Barras de Vida/Vigor/Mana (so aos 8, BarrasHud): uma linha logo abaixo da faixa de aviso, da coluna da
        /// esquerda (para onde o cartao de missao vai em tela baixa) ate antes da engrenagem do menu.</summary>
        public static Rect Barras(Rect safe, float alturaTela, float alvo, int fonte, int fonteAviso)
        {
            float m = alvo * 0.25f, h = fonte * 1.5f;
            float x0 = safe.x + safe.width * 0.2f + m, x1 = BotaoMenu(safe, alvo).xMin - m;
            return new Rect(x0, FaixaDeAviso(safe, alturaTela, fonteAviso).yMin - m * 0.5f - h, Mathf.Max(0f, x1 - x0), h);
        }

        /// <summary>Linha do painel do treino (so aos 8): alto e ao centro, abaixo do botao do salto e da faixa de aviso.</summary>
        public static Rect LinhaTreino(Rect safe)
        {
            return new Rect(safe.x + safe.width * 0.25f, safe.y + safe.height * 0.68f, safe.width * 0.5f, safe.height * 0.12f);
        }

        /// <summary>Prompt de interacao (nome do alvo): embaixo, no vao entre o joystick em repouso e os botoes.</summary>
        public static Rect Prompt(Rect safe, float dpi, ControlPreset p, int fonte)
        {
            float h = fonte * 1.6f, folga = h * 0.25f;
            List<Rect> toque = Toque(p, safe, dpi);
            Rect joystick = toque[toque.Count - 1];
            float x0, x1;
            if (p.hand == HandPreset.Destro)
            {
                x0 = joystick.xMax;
                x1 = safe.xMax;
                for (int i = 0; i < toque.Count - 1; i++) x1 = Mathf.Min(x1, toque[i].xMin);
            }
            else
            {
                x0 = safe.x;
                for (int i = 0; i < toque.Count - 1; i++) x0 = Mathf.Max(x0, toque[i].xMax);
                x1 = joystick.xMin;
            }
            return new Rect(x0 + folga, safe.y + h * 2f, Mathf.Max(0f, x1 - x0 - 2f * folga), h);
        }

        static Rect Quadrado(Vector2 c, float r) { return new Rect(c.x - r, c.y - r, 2f * r, 2f * r); }
    }
}
