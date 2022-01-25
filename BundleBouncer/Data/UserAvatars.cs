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
            switch(avType)
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