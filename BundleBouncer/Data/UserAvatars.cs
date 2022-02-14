/**
 * BundleBouncer User Avatar Info
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

namespace BundleBouncer.Data
{
    internal class UserAvatars : AvatarInfo
    {
        public string main = null;
        public string fallback = null;

        public void Set(EAvatarType avType, string avID)
        {
            switch (avType)
            {
                case EAvatarType.FALLBACK:
                    fallback = avID;
                    break;
                case EAvatarType.MAIN:
                    main = avID;
                    break;
            }
        }

        public string Get(EAvatarType avType)
        {
            switch (avType)
            {
                case EAvatarType.MAIN:
                    return main;
                case EAvatarType.FALLBACK:
                    return fallback;
            }
            return null;
        }

        public bool Contains(string avID)
        {
            return main == avID || fallback == avID;
        }
    }
}