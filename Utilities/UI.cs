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
                    using (var stream = typeof(BBUI).Assembly.GetManifestResourceStream("BundleBouncer.images.png.BundleBouncer-512.png"))
                    {
                        using (var memstream = new System.IO.MemoryStream((int)stream.Length))
                        {
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
            g.transform.Find("TitleText").localPosition = new Vector3(0f, 200f, 0f);
            g.transform.Find("TitleText").GetComponent<Text>().supportRichText = true;
            g.transform.Find("TitleText").GetComponent<Text>().text = text;
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
            Texture2D tex = new Texture2D(512, 512);
            if (!Il2CppImageConversionManager.LoadImage(tex, bytes)) return null;

            Sprite sprite = Sprite.CreateSprite(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f, 0, 0, new Vector4(), false);
            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return sprite;
        }
    }
}
