/**
 * BundleBouncer UI Interfaces
 * 
 * Copyright (c) 2022 BundleBouncer Contributors
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using MelonLoader;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BundleBouncer.Utilities
{
    public static class BBUI
    {
        private static Sprite _bbIcon;

        public static Sprite bbIcon
        {
            get
            {
                if (_bbIcon == null)
                {
                    using (var stream = typeof(BundleBouncer).Assembly.GetManifestResourceStream(typeof(BundleBouncer), "images.png.BundleBouncer-512.png"))
                    {
                        using (var memstream = new System.IO.MemoryStream((int)stream.Length))
                        {
                            stream.CopyTo(memstream);
                            _bbIcon = LoadSpriteFromBytes(memstream.ToArray());
                        }
                    }
                }
                return _bbIcon;
            }
        }

        public static void NotifyUser(string text)
        {
            text = text.Replace("<", "<<"); // No XSS for you
            // TODO: Switch between available notification systems, like VRCX or XSOverlay
            MelonCoroutines.Start(CreatePopupV2(text, bbIcon));
        }

        public static IEnumerator CreatePopupV2(string text, Sprite sprite = null, float time = 5f)
        {
            GameObject g = GameObject.Instantiate(GameObject.Find("UserInterface/MenuContent/Popups/InputPopup/"), GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud/").transform);
            g.transform.localScale = new Vector3(0f, 0.5f, 0.5f);
            g.transform.localPosition = new Vector3(0f, -400f, 0f);
            g.GetComponent<VRCUiPopupInput>().enabled = false;
            g.GetComponent<CanvasGroup>().enabled = false;
            g.transform.Find("InputField").gameObject.SetActive(false);
            g.transform.Find("Keyboard").gameObject.SetActive(false);
            g.transform.Find("ButtonLeft").gameObject.SetActive(false);
            g.transform.Find("ButtonRight").gameObject.SetActive(false);
            g.transform.Find("ButtonCenter").gameObject.SetActive(false);
            g.transform.Find("PasswordVisibilityToggle").localPosition = new Vector3(-420f, 200f, 0);
            g.transform.Find("Darkness").localPosition = new Vector3(0f, 200f, 0f);
            g.transform.Find("Darkness").localScale = new Vector3(1f, 0.1f, 0.5f);
            var tt = g.transform.Find("TitleText");
            {
                tt.localPosition = new Vector3(0f, 200f, 0f);
                var ttt = tt.GetComponent<Text>();
                {
                    ttt.supportRichText = true;
                    ttt.text = text;
                }
            }
            g.transform.Find("Rectangle").gameObject.SetActive(false);
            g.transform.Find("CharactersRemainingText").gameObject.SetActive(false);
            g.transform.Find("PasswordVisibilityToggle").GetComponent<Image>().sprite = sprite;
            g.SetActive(true);
            if (g.transform.Find("ButtonPaste") != null) g.transform.Find("ButtonPaste").gameObject.SetActive(false);

            while (g.transform.localScale.x < 0.5f && g != null)
            {
                yield return new WaitForSeconds(0.02f);
                g.transform.localScale = new Vector3(g.transform.localScale.x + 0.05f, g.transform.localScale.y, g.transform.localScale.z);
                if (g.transform.localScale.x > 0.5f)
                    yield return null;
            }

            yield return new WaitForSeconds(time);
            while (g.transform.localScale.x > 0f && g != null)
            {
                yield return new WaitForSeconds(0.02f);
                g.transform.localScale = new Vector3(g.transform.localScale.x - 0.05f, g.transform.localScale.y, g.transform.localScale.z);
                if (g.transform.localScale.x == 0f || g.transform.localScale.x < 0f)
                {
                    GameObject.Destroy(g);
                    yield break;
                }
            }

            yield break;
        }
        public static Sprite LoadSpriteFromBytes(byte[] bytes)
        {
            Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            if (!ImageConversion.LoadImage(tex, bytes)) {
                Logging.Warning("Failed to load sprite");
                return null;
            }
            tex.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            tex.wrapMode = TextureWrapMode.Clamp;

            var rect = new Rect(0.0f, 0.0f, tex.width, tex.height);
            var anchor = new Vector2(0.5f, 0.5f);
            var border = new Vector4();
            Sprite sprite = Sprite.CreateSprite_Injected(tex, ref rect, ref anchor, 100.0f, 0, 0, ref border, false);
            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return sprite;
        }
    }
}
