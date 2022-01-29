﻿using BundleBouncer.Data;

namespace BundleBouncer.Shitlist
{
    // Tell FxCop and others to STFU
    /// <auto-generated></auto-generated>
    [System.CodeDom.Compiler.GeneratedCode("devtools/build.py", "0.0.0")]
    public class ShitlistProvider : IShitListProvider
    {
        public ShitlistProvider() => Logging.Info("BundleBouncer definitions generated @ 2022-01-29T00:29:54.266222");

        // The following is a bunch of generated if-trees created by putting
        // a bunch of avID SHA256s into a trie (https://en.wikipedia.org/wiki/Trie) and optimizing it.
        // Why?  Because I wanted to. Also offers some level of obfuscation.
        // Mostly the cool factor, though.
        bool IShitListProvider.IsAssetBundleAnAssetBundleCrasher(byte[] digest)
        {
            if(digest[0]==154 && digest[1]==141 && digest[2]==180 && digest[3]==127 && digest[4]==219 && digest[5]==180 && digest[6]==176 && digest[7]==128 && digest[8]==45 && digest[9]==214 && digest[10]==72 && digest[11]==244 && digest[12]==239 && digest[13]==171 && digest[14]==230 && digest[15]==27 && digest[16]==133 && digest[17]==2 && digest[18]==97 && digest[19]==79 && digest[20]==232 && digest[21]==110 && digest[22]==151 && digest[23]==160 && digest[24]==124 && digest[25]==253 && digest[26]==26 && digest[27]==230 && digest[28]==241 && digest[29]==158 && digest[30]==209 && digest[31]==131)
            {
                return true;
            }
            else if(digest[0]==158 && digest[1]==115 && digest[2]==178 && digest[3]==240 && digest[4]==164 && digest[5]==1 && digest[6]==254 && digest[7]==103 && digest[8]==9 && digest[9]==20 && digest[10]==34 && digest[11]==172 && digest[12]==131 && digest[13]==48 && digest[14]==213 && digest[15]==19 && digest[16]==139 && digest[17]==198 && digest[18]==93 && digest[19]==175 && digest[20]==36 && digest[21]==228 && digest[22]==53 && digest[23]==100 && digest[24]==153 && digest[25]==44 && digest[26]==152 && digest[27]==1 && digest[28]==249 && digest[29]==226 && digest[30]==103 && digest[31]==27)
            {
                return true;
            }
            else if(digest[0]==160 && digest[1]==13 && digest[2]==47 && digest[3]==153 && digest[4]==37 && digest[5]==44 && digest[6]==90 && digest[7]==163 && digest[8]==151 && digest[9]==126 && digest[10]==127 && digest[11]==126 && digest[12]==192 && digest[13]==228 && digest[14]==160 && digest[15]==93 && digest[16]==60 && digest[17]==237 && digest[18]==144 && digest[19]==196 && digest[20]==222 && digest[21]==152 && digest[22]==226 && digest[23]==32 && digest[24]==117 && digest[25]==245 && digest[26]==233 && digest[27]==84 && digest[28]==200 && digest[29]==137 && digest[30]==189 && digest[31]==96)
            {
                return true;
            }
            else if(digest[0]==190)
            {
                if(digest[1]==200 && digest[2]==202 && digest[3]==196 && digest[4]==20 && digest[5]==30 && digest[6]==200 && digest[7]==232 && digest[8]==177 && digest[9]==202 && digest[10]==56 && digest[11]==124 && digest[12]==15 && digest[13]==108 && digest[14]==135 && digest[15]==204 && digest[16]==116 && digest[17]==178 && digest[18]==90 && digest[19]==71 && digest[20]==232 && digest[21]==98 && digest[22]==152 && digest[23]==82 && digest[24]==121 && digest[25]==35 && digest[26]==114 && digest[27]==26 && digest[28]==179 && digest[29]==228 && digest[30]==245 && digest[31]==40)
                {
                    return true;
                }
                else if(digest[1]==24 && digest[2]==140 && digest[3]==196 && digest[4]==194 && digest[5]==105 && digest[6]==40 && digest[7]==41 && digest[8]==74 && digest[9]==89 && digest[10]==63 && digest[11]==188 && digest[12]==17 && digest[13]==141 && digest[14]==142 && digest[15]==111 && digest[16]==40 && digest[17]==124 && digest[18]==89 && digest[19]==24 && digest[20]==215 && digest[21]==91 && digest[22]==209 && digest[23]==85 && digest[24]==187 && digest[25]==200 && digest[26]==70 && digest[27]==12 && digest[28]==175 && digest[29]==26 && digest[30]==206 && digest[31]==25)
                {
                    return true;
                }
            }
            else if(digest[0]==196 && digest[1]==210 && digest[2]==187 && digest[3]==85 && digest[4]==106 && digest[5]==30 && digest[6]==93 && digest[7]==4 && digest[8]==163 && digest[9]==128 && digest[10]==21 && digest[11]==75 && digest[12]==62 && digest[13]==106 && digest[14]==251 && digest[15]==245 && digest[16]==170 && digest[17]==156 && digest[18]==146 && digest[19]==184 && digest[20]==153 && digest[21]==147 && digest[22]==215 && digest[23]==119 && digest[24]==169 && digest[25]==136 && digest[26]==249 && digest[27]==107 && digest[28]==121 && digest[29]==183 && digest[30]==199 && digest[31]==177)
            {
                return true;
            }
            else if(digest[0]==200 && digest[1]==94 && digest[2]==8 && digest[3]==177 && digest[4]==182 && digest[5]==220 && digest[6]==237 && digest[7]==28 && digest[8]==251 && digest[9]==228 && digest[10]==176 && digest[11]==114 && digest[12]==125 && digest[13]==190 && digest[14]==227 && digest[15]==168 && digest[16]==122 && digest[17]==218 && digest[18]==91 && digest[19]==79 && digest[20]==251 && digest[21]==130 && digest[22]==18 && digest[23]==85 && digest[24]==25 && digest[25]==75 && digest[26]==61 && digest[27]==65 && digest[28]==55 && digest[29]==139 && digest[30]==97 && digest[31]==132)
            {
                return true;
            }
            else if(digest[0]==201 && digest[1]==211 && digest[2]==11 && digest[3]==197 && digest[4]==242 && digest[5]==184 && digest[6]==111 && digest[7]==57 && digest[8]==120 && digest[9]==137 && digest[10]==158 && digest[11]==155 && digest[12]==174 && digest[13]==6 && digest[14]==22 && digest[15]==127 && digest[16]==101 && digest[17]==174 && digest[18]==177 && digest[19]==60 && digest[20]==177 && digest[21]==33 && digest[22]==153 && digest[23]==117 && digest[24]==227 && digest[25]==157 && digest[26]==129 && digest[27]==134 && digest[28]==135 && digest[29]==199 && digest[30]==183 && digest[31]==106)
            {
                return true;
            }
            else if(digest[0]==222 && digest[1]==60 && digest[2]==68 && digest[3]==218 && digest[4]==212 && digest[5]==31 && digest[6]==109 && digest[7]==67 && digest[8]==97 && digest[9]==43 && digest[10]==3 && digest[11]==158 && digest[12]==45 && digest[13]==107 && digest[14]==205 && digest[15]==193 && digest[16]==248 && digest[17]==122 && digest[18]==148 && digest[19]==48 && digest[20]==119 && digest[21]==194 && digest[22]==245 && digest[23]==14 && digest[24]==178 && digest[25]==232 && digest[26]==139 && digest[27]==94 && digest[28]==107 && digest[29]==62 && digest[30]==247 && digest[31]==251)
            {
                return true;
            }
            else if(digest[0]==224 && digest[1]==141 && digest[2]==209 && digest[3]==187 && digest[4]==210 && digest[5]==207 && digest[6]==61 && digest[7]==230 && digest[8]==109 && digest[9]==9 && digest[10]==247 && digest[11]==187 && digest[12]==148 && digest[13]==235 && digest[14]==79 && digest[15]==98 && digest[16]==137 && digest[17]==45 && digest[18]==3 && digest[19]==137 && digest[20]==44 && digest[21]==92 && digest[22]==114 && digest[23]==8 && digest[24]==177 && digest[25]==172 && digest[26]==11 && digest[27]==55 && digest[28]==6 && digest[29]==171 && digest[30]==66 && digest[31]==208)
            {
                return true;
            }
            else if(digest[0]==23 && digest[1]==91 && digest[2]==89 && digest[3]==145 && digest[4]==44 && digest[5]==254 && digest[6]==149 && digest[7]==164 && digest[8]==11 && digest[9]==200 && digest[10]==13 && digest[11]==206 && digest[12]==91 && digest[13]==109 && digest[14]==255 && digest[15]==32 && digest[16]==1 && digest[17]==250 && digest[18]==0 && digest[19]==217 && digest[20]==82 && digest[21]==72 && digest[22]==143 && digest[23]==5 && digest[24]==164 && digest[25]==168 && digest[26]==116 && digest[27]==186 && digest[28]==65 && digest[29]==69 && digest[30]==94 && digest[31]==209)
            {
                return true;
            }
            else if(digest[0]==33 && digest[1]==65 && digest[2]==235 && digest[3]==225 && digest[4]==247 && digest[5]==75 && digest[6]==189 && digest[7]==106 && digest[8]==229 && digest[9]==186 && digest[10]==124 && digest[11]==244 && digest[12]==175 && digest[13]==47 && digest[14]==20 && digest[15]==29 && digest[16]==251 && digest[17]==52 && digest[18]==127 && digest[19]==154 && digest[20]==142 && digest[21]==160 && digest[22]==146 && digest[23]==63 && digest[24]==233 && digest[25]==234 && digest[26]==90 && digest[27]==174 && digest[28]==127 && digest[29]==71 && digest[30]==169 && digest[31]==190)
            {
                return true;
            }
            else if(digest[0]==37 && digest[1]==160 && digest[2]==61 && digest[3]==33 && digest[4]==196 && digest[5]==130 && digest[6]==127 && digest[7]==64 && digest[8]==88 && digest[9]==68 && digest[10]==197 && digest[11]==0 && digest[12]==19 && digest[13]==198 && digest[14]==66 && digest[15]==89 && digest[16]==67 && digest[17]==232 && digest[18]==64 && digest[19]==185 && digest[20]==242 && digest[21]==29 && digest[22]==177 && digest[23]==255 && digest[24]==194 && digest[25]==44 && digest[26]==200 && digest[27]==24 && digest[28]==243 && digest[29]==197 && digest[30]==82 && digest[31]==112)
            {
                return true;
            }
            else if(digest[0]==41 && digest[1]==84 && digest[2]==151 && digest[3]==213 && digest[4]==254 && digest[5]==162 && digest[6]==63 && digest[7]==69 && digest[8]==113 && digest[9]==239 && digest[10]==65 && digest[11]==116 && digest[12]==147 && digest[13]==225 && digest[14]==34 && digest[15]==162 && digest[16]==156 && digest[17]==178 && digest[18]==39 && digest[19]==210 && digest[20]==9 && digest[21]==218 && digest[22]==229 && digest[23]==182 && digest[24]==162 && digest[25]==143 && digest[26]==162 && digest[27]==253 && digest[28]==101 && digest[29]==77 && digest[30]==49 && digest[31]==147)
            {
                return true;
            }
            else if(digest[0]==49 && digest[1]==138 && digest[2]==217 && digest[3]==143 && digest[4]==49 && digest[5]==74 && digest[6]==166 && digest[7]==237 && digest[8]==52 && digest[9]==170 && digest[10]==125 && digest[11]==216 && digest[12]==224 && digest[13]==55 && digest[14]==140 && digest[15]==104 && digest[16]==66 && digest[17]==208 && digest[18]==150 && digest[19]==87 && digest[20]==7 && digest[21]==107 && digest[22]==0 && digest[23]==130 && digest[24]==105 && digest[25]==183 && digest[26]==30 && digest[27]==198 && digest[28]==15 && digest[29]==93 && digest[30]==18 && digest[31]==212)
            {
                return true;
            }
            else if(digest[0]==56 && digest[1]==123 && digest[2]==179 && digest[3]==238 && digest[4]==40 && digest[5]==110 && digest[6]==14 && digest[7]==255 && digest[8]==198 && digest[9]==219 && digest[10]==30 && digest[11]==28 && digest[12]==89 && digest[13]==177 && digest[14]==57 && digest[15]==253 && digest[16]==101 && digest[17]==169 && digest[18]==91 && digest[19]==150 && digest[20]==207 && digest[21]==138 && digest[22]==148 && digest[23]==167 && digest[24]==169 && digest[25]==68 && digest[26]==69 && digest[27]==43 && digest[28]==172 && digest[29]==232 && digest[30]==61 && digest[31]==35)
            {
                return true;
            }
            else if(digest[0]==6 && digest[1]==108 && digest[2]==121 && digest[3]==72 && digest[4]==78 && digest[5]==58 && digest[6]==86 && digest[7]==26 && digest[8]==155 && digest[9]==251 && digest[10]==237 && digest[11]==46 && digest[12]==55 && digest[13]==232 && digest[14]==4 && digest[15]==144 && digest[16]==177 && digest[17]==45 && digest[18]==209 && digest[19]==217 && digest[20]==102 && digest[21]==104 && digest[22]==42 && digest[23]==59 && digest[24]==24 && digest[25]==79 && digest[26]==227 && digest[27]==246 && digest[28]==220 && digest[29]==79 && digest[30]==25 && digest[31]==124)
            {
                return true;
            }
            else if(digest[0]==62 && digest[1]==10 && digest[2]==35 && digest[3]==215 && digest[4]==148 && digest[5]==185 && digest[6]==5 && digest[7]==33 && digest[8]==113 && digest[9]==217 && digest[10]==138 && digest[11]==227 && digest[12]==194 && digest[13]==252 && digest[14]==170 && digest[15]==205 && digest[16]==108 && digest[17]==180 && digest[18]==21 && digest[19]==157 && digest[20]==219 && digest[21]==129 && digest[22]==94 && digest[23]==31 && digest[24]==107 && digest[25]==239 && digest[26]==195 && digest[27]==120 && digest[28]==211 && digest[29]==22 && digest[30]==80 && digest[31]==227)
            {
                return true;
            }
            else if(digest[0]==65 && digest[1]==220 && digest[2]==160 && digest[3]==102 && digest[4]==206 && digest[5]==78 && digest[6]==161 && digest[7]==255 && digest[8]==8 && digest[9]==67 && digest[10]==177 && digest[11]==246 && digest[12]==21 && digest[13]==55 && digest[14]==72 && digest[15]==192 && digest[16]==8 && digest[17]==202 && digest[18]==91 && digest[19]==190 && digest[20]==190 && digest[21]==48 && digest[22]==189 && digest[23]==190 && digest[24]==45 && digest[25]==138 && digest[26]==15 && digest[27]==8 && digest[28]==73 && digest[29]==120 && digest[30]==212 && digest[31]==1)
            {
                return true;
            }
            else if(digest[0]==81 && digest[1]==114 && digest[2]==201 && digest[3]==12 && digest[4]==21 && digest[5]==165 && digest[6]==76 && digest[7]==247 && digest[8]==17 && digest[9]==5 && digest[10]==190 && digest[11]==68 && digest[12]==212 && digest[13]==225 && digest[14]==143 && digest[15]==128 && digest[16]==177 && digest[17]==196 && digest[18]==207 && digest[19]==3 && digest[20]==86 && digest[21]==249 && digest[22]==236 && digest[23]==38 && digest[24]==241 && digest[25]==39 && digest[26]==231 && digest[27]==8 && digest[28]==207 && digest[29]==4 && digest[30]==200 && digest[31]==125)
            {
                return true;
            }
            else if(digest[0]==83 && digest[1]==158 && digest[2]==57 && digest[3]==85 && digest[4]==56 && digest[5]==130 && digest[6]==241 && digest[7]==99 && digest[8]==144 && digest[9]==117 && digest[10]==24 && digest[11]==184 && digest[12]==77 && digest[13]==19 && digest[14]==119 && digest[15]==201 && digest[16]==18 && digest[17]==83 && digest[18]==164 && digest[19]==150 && digest[20]==214 && digest[21]==176 && digest[22]==188 && digest[23]==168 && digest[24]==176 && digest[25]==252 && digest[26]==202 && digest[27]==105 && digest[28]==236 && digest[29]==51 && digest[30]==110 && digest[31]==155)
            {
                return true;
            }

            return false;
        }

        bool IShitListProvider.IsAvatarIDAnAssetBundleCrasher(byte[] digest)
        {
            if(digest[0]==100 && digest[1]==147 && digest[2]==108 && digest[3]==93 && digest[4]==106 && digest[5]==104 && digest[6]==243 && digest[7]==91 && digest[8]==253 && digest[9]==127 && digest[10]==35 && digest[11]==188 && digest[12]==104 && digest[13]==72 && digest[14]==129 && digest[15]==121 && digest[16]==140 && digest[17]==240 && digest[18]==250 && digest[19]==11 && digest[20]==148 && digest[21]==162 && digest[22]==23 && digest[23]==132 && digest[24]==158 && digest[25]==36 && digest[26]==11 && digest[27]==111 && digest[28]==131 && digest[29]==236 && digest[30]==96 && digest[31]==107)
            {
                return true;
            }
            else if(digest[0]==123 && digest[1]==85 && digest[2]==60 && digest[3]==225 && digest[4]==63 && digest[5]==37 && digest[6]==177 && digest[7]==232 && digest[8]==171 && digest[9]==180 && digest[10]==69 && digest[11]==124 && digest[12]==22 && digest[13]==61 && digest[14]==54 && digest[15]==120 && digest[16]==5 && digest[17]==66 && digest[18]==242 && digest[19]==216 && digest[20]==216 && digest[21]==20 && digest[22]==87 && digest[23]==80 && digest[24]==57 && digest[25]==247 && digest[26]==255 && digest[27]==250 && digest[28]==148 && digest[29]==149 && digest[30]==49 && digest[31]==181)
            {
                return true;
            }
            else if(digest[0]==138)
            {
                if(digest[1]==66 && digest[2]==130 && digest[3]==108 && digest[4]==8 && digest[5]==162 && digest[6]==176 && digest[7]==180 && digest[8]==220 && digest[9]==9 && digest[10]==1 && digest[11]==139 && digest[12]==251 && digest[13]==146 && digest[14]==99 && digest[15]==124 && digest[16]==197 && digest[17]==190 && digest[18]==250 && digest[19]==181 && digest[20]==170 && digest[21]==12 && digest[22]==225 && digest[23]==212 && digest[24]==121 && digest[25]==10 && digest[26]==248 && digest[27]==197 && digest[28]==124 && digest[29]==53 && digest[30]==53 && digest[31]==50)
                {
                    return true;
                }
                else if(digest[1]==74 && digest[2]==149 && digest[3]==96 && digest[4]==189 && digest[5]==1 && digest[6]==241 && digest[7]==150 && digest[8]==145 && digest[9]==224 && digest[10]==139 && digest[11]==125 && digest[12]==183 && digest[13]==20 && digest[14]==2 && digest[15]==53 && digest[16]==43 && digest[17]==107 && digest[18]==145 && digest[19]==239 && digest[20]==114 && digest[21]==67 && digest[22]==110 && digest[23]==150 && digest[24]==77 && digest[25]==131 && digest[26]==50 && digest[27]==142 && digest[28]==110 && digest[29]==40 && digest[30]==232 && digest[31]==28)
                {
                    return true;
                }
            }
            else if(digest[0]==142 && digest[1]==176 && digest[2]==130 && digest[3]==101 && digest[4]==142 && digest[5]==161 && digest[6]==56 && digest[7]==62 && digest[8]==28 && digest[9]==241 && digest[10]==205 && digest[11]==165 && digest[12]==56 && digest[13]==173 && digest[14]==117 && digest[15]==222 && digest[16]==107 && digest[17]==201 && digest[18]==161 && digest[19]==56 && digest[20]==81 && digest[21]==108 && digest[22]==191 && digest[23]==121 && digest[24]==159 && digest[25]==18 && digest[26]==124 && digest[27]==116 && digest[28]==201 && digest[29]==144 && digest[30]==93 && digest[31]==213)
            {
                return true;
            }
            else if(digest[0]==150 && digest[1]==221 && digest[2]==114 && digest[3]==8 && digest[4]==85 && digest[5]==135 && digest[6]==158 && digest[7]==73 && digest[8]==240 && digest[9]==201 && digest[10]==0 && digest[11]==96 && digest[12]==112 && digest[13]==205 && digest[14]==180 && digest[15]==163 && digest[16]==139 && digest[17]==163 && digest[18]==73 && digest[19]==145 && digest[20]==219 && digest[21]==171 && digest[22]==89 && digest[23]==31 && digest[24]==143 && digest[25]==123 && digest[26]==92 && digest[27]==194 && digest[28]==232 && digest[29]==184 && digest[30]==209 && digest[31]==91)
            {
                return true;
            }
            else if(digest[0]==157 && digest[1]==245 && digest[2]==148 && digest[3]==214 && digest[4]==125 && digest[5]==13 && digest[6]==18 && digest[7]==189 && digest[8]==245 && digest[9]==80 && digest[10]==208 && digest[11]==17 && digest[12]==11 && digest[13]==102 && digest[14]==146 && digest[15]==82 && digest[16]==201 && digest[17]==195 && digest[18]==190 && digest[19]==76 && digest[20]==32 && digest[21]==197 && digest[22]==17 && digest[23]==172 && digest[24]==112 && digest[25]==85 && digest[26]==72 && digest[27]==115 && digest[28]==115 && digest[29]==30 && digest[30]==110 && digest[31]==134)
            {
                return true;
            }
            else if(digest[0]==167 && digest[1]==5 && digest[2]==217 && digest[3]==220 && digest[4]==19 && digest[5]==46 && digest[6]==205 && digest[7]==63 && digest[8]==234 && digest[9]==70 && digest[10]==156 && digest[11]==48 && digest[12]==112 && digest[13]==119 && digest[14]==151 && digest[15]==60 && digest[16]==187 && digest[17]==57 && digest[18]==56 && digest[19]==9 && digest[20]==158 && digest[21]==211 && digest[22]==186 && digest[23]==197 && digest[24]==175 && digest[25]==177 && digest[26]==47 && digest[27]==191 && digest[28]==5 && digest[29]==125 && digest[30]==227 && digest[31]==213)
            {
                return true;
            }
            else if(digest[0]==170 && digest[1]==206 && digest[2]==197 && digest[3]==9 && digest[4]==90 && digest[5]==124 && digest[6]==182 && digest[7]==135 && digest[8]==80 && digest[9]==9 && digest[10]==180 && digest[11]==164 && digest[12]==201 && digest[13]==187 && digest[14]==232 && digest[15]==143 && digest[16]==118 && digest[17]==93 && digest[18]==120 && digest[19]==65 && digest[20]==194 && digest[21]==175 && digest[22]==3 && digest[23]==178 && digest[24]==38 && digest[25]==113 && digest[26]==65 && digest[27]==19 && digest[28]==44 && digest[29]==245 && digest[30]==60 && digest[31]==219)
            {
                return true;
            }
            else if(digest[0]==185 && digest[1]==31 && digest[2]==203 && digest[3]==133 && digest[4]==81 && digest[5]==222 && digest[6]==49 && digest[7]==106 && digest[8]==43 && digest[9]==41 && digest[10]==12 && digest[11]==119 && digest[12]==242 && digest[13]==206 && digest[14]==3 && digest[15]==107 && digest[16]==195 && digest[17]==255 && digest[18]==254 && digest[19]==255 && digest[20]==82 && digest[21]==30 && digest[22]==55 && digest[23]==218 && digest[24]==237 && digest[25]==226 && digest[26]==120 && digest[27]==68 && digest[28]==13 && digest[29]==43 && digest[30]==89 && digest[31]==146)
            {
                return true;
            }
            else if(digest[0]==196 && digest[1]==157 && digest[2]==59 && digest[3]==86 && digest[4]==236 && digest[5]==227 && digest[6]==97 && digest[7]==210 && digest[8]==75 && digest[9]==28 && digest[10]==88 && digest[11]==87 && digest[12]==87 && digest[13]==2 && digest[14]==215 && digest[15]==242 && digest[16]==89 && digest[17]==255 && digest[18]==149 && digest[19]==81 && digest[20]==9 && digest[21]==153 && digest[22]==144 && digest[23]==116 && digest[24]==134 && digest[25]==242 && digest[26]==163 && digest[27]==33 && digest[28]==144 && digest[29]==231 && digest[30]==44 && digest[31]==216)
            {
                return true;
            }
            else if(digest[0]==197 && digest[1]==88 && digest[2]==241 && digest[3]==121 && digest[4]==16 && digest[5]==173 && digest[6]==33 && digest[7]==208 && digest[8]==74 && digest[9]==25 && digest[10]==228 && digest[11]==165 && digest[12]==222 && digest[13]==63 && digest[14]==192 && digest[15]==37 && digest[16]==151 && digest[17]==60 && digest[18]==140 && digest[19]==205 && digest[20]==209 && digest[21]==11 && digest[22]==51 && digest[23]==177 && digest[24]==165 && digest[25]==235 && digest[26]==195 && digest[27]==101 && digest[28]==16 && digest[29]==231 && digest[30]==150 && digest[31]==8)
            {
                return true;
            }
            else if(digest[0]==2 && digest[1]==215 && digest[2]==19 && digest[3]==12 && digest[4]==142 && digest[5]==80 && digest[6]==43 && digest[7]==131 && digest[8]==95 && digest[9]==71 && digest[10]==190 && digest[11]==208 && digest[12]==227 && digest[13]==16 && digest[14]==249 && digest[15]==142 && digest[16]==250 && digest[17]==64 && digest[18]==165 && digest[19]==150 && digest[20]==94 && digest[21]==58 && digest[22]==157 && digest[23]==211 && digest[24]==233 && digest[25]==26 && digest[26]==144 && digest[27]==208 && digest[28]==247 && digest[29]==81 && digest[30]==40 && digest[31]==4)
            {
                return true;
            }
            else if(digest[0]==218 && digest[1]==97 && digest[2]==148 && digest[3]==82 && digest[4]==229 && digest[5]==69 && digest[6]==30 && digest[7]==149 && digest[8]==180 && digest[9]==123 && digest[10]==45 && digest[11]==126 && digest[12]==51 && digest[13]==111 && digest[14]==41 && digest[15]==226 && digest[16]==233 && digest[17]==244 && digest[18]==206 && digest[19]==65 && digest[20]==57 && digest[21]==195 && digest[22]==177 && digest[23]==120 && digest[24]==184 && digest[25]==56 && digest[26]==249 && digest[27]==139 && digest[28]==28 && digest[29]==70 && digest[30]==159 && digest[31]==13)
            {
                return true;
            }
            else if(digest[0]==220 && digest[1]==72 && digest[2]==240 && digest[3]==1 && digest[4]==189 && digest[5]==129 && digest[6]==248 && digest[7]==145 && digest[8]==108 && digest[9]==29 && digest[10]==145 && digest[11]==154 && digest[12]==146 && digest[13]==194 && digest[14]==238 && digest[15]==189 && digest[16]==16 && digest[17]==21 && digest[18]==61 && digest[19]==63 && digest[20]==228 && digest[21]==22 && digest[22]==132 && digest[23]==5 && digest[24]==226 && digest[25]==4 && digest[26]==16 && digest[27]==41 && digest[28]==99 && digest[29]==61 && digest[30]==211 && digest[31]==222)
            {
                return true;
            }
            else if(digest[0]==225 && digest[1]==63 && digest[2]==231 && digest[3]==56 && digest[4]==152 && digest[5]==169 && digest[6]==56 && digest[7]==241 && digest[8]==151 && digest[9]==134 && digest[10]==42 && digest[11]==207 && digest[12]==167 && digest[13]==61 && digest[14]==97 && digest[15]==13 && digest[16]==7 && digest[17]==154 && digest[18]==8 && digest[19]==46 && digest[20]==139 && digest[21]==202 && digest[22]==73 && digest[23]==124 && digest[24]==254 && digest[25]==64 && digest[26]==124 && digest[27]==32 && digest[28]==41 && digest[29]==54 && digest[30]==197 && digest[31]==65)
            {
                return true;
            }
            else if(digest[0]==230 && digest[1]==35 && digest[2]==123 && digest[3]==232 && digest[4]==248 && digest[5]==187 && digest[6]==243 && digest[7]==43 && digest[8]==98 && digest[9]==175 && digest[10]==44 && digest[11]==91 && digest[12]==93 && digest[13]==254 && digest[14]==74 && digest[15]==89 && digest[16]==182 && digest[17]==34 && digest[18]==35 && digest[19]==231 && digest[20]==203 && digest[21]==235 && digest[22]==66 && digest[23]==166 && digest[24]==19 && digest[25]==12 && digest[26]==210 && digest[27]==26 && digest[28]==138 && digest[29]==106 && digest[30]==2 && digest[31]==217)
            {
                return true;
            }
            else if(digest[0]==247 && digest[1]==238 && digest[2]==132 && digest[3]==20 && digest[4]==91 && digest[5]==157 && digest[6]==86 && digest[7]==155 && digest[8]==208 && digest[9]==181 && digest[10]==77 && digest[11]==10 && digest[12]==58 && digest[13]==36 && digest[14]==28 && digest[15]==54 && digest[16]==151 && digest[17]==255 && digest[18]==243 && digest[19]==193 && digest[20]==74 && digest[21]==199 && digest[22]==114 && digest[23]==144 && digest[24]==224 && digest[25]==27 && digest[26]==220 && digest[27]==56 && digest[28]==247 && digest[29]==39 && digest[30]==11 && digest[31]==188)
            {
                return true;
            }
            else if(digest[0]==29 && digest[1]==104 && digest[2]==115 && digest[3]==26 && digest[4]==61 && digest[5]==182 && digest[6]==171 && digest[7]==109 && digest[8]==112 && digest[9]==209 && digest[10]==228 && digest[11]==214 && digest[12]==152 && digest[13]==161 && digest[14]==187 && digest[15]==199 && digest[16]==242 && digest[17]==238 && digest[18]==152 && digest[19]==40 && digest[20]==77 && digest[21]==213 && digest[22]==21 && digest[23]==247 && digest[24]==245 && digest[25]==102 && digest[26]==148 && digest[27]==94 && digest[28]==101 && digest[29]==37 && digest[30]==113 && digest[31]==46)
            {
                return true;
            }
            else if(digest[0]==43 && digest[1]==93 && digest[2]==39 && digest[3]==140 && digest[4]==165 && digest[5]==26 && digest[6]==94 && digest[7]==75 && digest[8]==253 && digest[9]==133 && digest[10]==68 && digest[11]==195 && digest[12]==156 && digest[13]==134 && digest[14]==114 && digest[15]==250 && digest[16]==148 && digest[17]==59 && digest[18]==104 && digest[19]==114 && digest[20]==226 && digest[21]==201 && digest[22]==191 && digest[23]==176 && digest[24]==148 && digest[25]==128 && digest[26]==84 && digest[27]==65 && digest[28]==237 && digest[29]==240 && digest[30]==47 && digest[31]==56)
            {
                return true;
            }
            else if(digest[0]==44 && digest[1]==87 && digest[2]==138 && digest[3]==37 && digest[4]==223 && digest[5]==167 && digest[6]==176 && digest[7]==207 && digest[8]==43 && digest[9]==165 && digest[10]==185 && digest[11]==99 && digest[12]==80 && digest[13]==144 && digest[14]==214 && digest[15]==66 && digest[16]==101 && digest[17]==249 && digest[18]==51 && digest[19]==64 && digest[20]==132 && digest[21]==132 && digest[22]==92 && digest[23]==146 && digest[24]==130 && digest[25]==10 && digest[26]==94 && digest[27]==193 && digest[28]==11 && digest[29]==157 && digest[30]==53 && digest[31]==4)
            {
                return true;
            }
            else if(digest[0]==47 && digest[1]==131 && digest[2]==149 && digest[3]==175 && digest[4]==209 && digest[5]==228 && digest[6]==154 && digest[7]==30 && digest[8]==115 && digest[9]==177 && digest[10]==117 && digest[11]==197 && digest[12]==99 && digest[13]==150 && digest[14]==22 && digest[15]==39 && digest[16]==0 && digest[17]==162 && digest[18]==174 && digest[19]==246 && digest[20]==82 && digest[21]==86 && digest[22]==116 && digest[23]==118 && digest[24]==76 && digest[25]==195 && digest[26]==210 && digest[27]==124 && digest[28]==255 && digest[29]==107 && digest[30]==23 && digest[31]==99)
            {
                return true;
            }
            else if(digest[0]==48 && digest[1]==91 && digest[2]==63 && digest[3]==231 && digest[4]==172 && digest[5]==165 && digest[6]==210 && digest[7]==98 && digest[8]==50 && digest[9]==205 && digest[10]==60 && digest[11]==63 && digest[12]==110 && digest[13]==167 && digest[14]==102 && digest[15]==89 && digest[16]==31 && digest[17]==214 && digest[18]==87 && digest[19]==42 && digest[20]==56 && digest[21]==27 && digest[22]==164 && digest[23]==110 && digest[24]==211 && digest[25]==236 && digest[26]==137 && digest[27]==229 && digest[28]==169 && digest[29]==237 && digest[30]==209 && digest[31]==83)
            {
                return true;
            }
            else if(digest[0]==57)
            {
                if(digest[1]==147 && digest[2]==233 && digest[3]==30 && digest[4]==83 && digest[5]==136 && digest[6]==239 && digest[7]==55 && digest[8]==121 && digest[9]==32 && digest[10]==143 && digest[11]==232 && digest[12]==160 && digest[13]==29 && digest[14]==155 && digest[15]==156 && digest[16]==94 && digest[17]==215 && digest[18]==83 && digest[19]==29 && digest[20]==162 && digest[21]==40 && digest[22]==110 && digest[23]==41 && digest[24]==255 && digest[25]==27 && digest[26]==154 && digest[27]==251 && digest[28]==82 && digest[29]==30 && digest[30]==68 && digest[31]==39)
                {
                    return true;
                }
                else if(digest[1]==169 && digest[2]==6 && digest[3]==177 && digest[4]==119 && digest[5]==190 && digest[6]==33 && digest[7]==240 && digest[8]==212 && digest[9]==242 && digest[10]==178 && digest[11]==116 && digest[12]==191 && digest[13]==147 && digest[14]==201 && digest[15]==221 && digest[16]==196 && digest[17]==32 && digest[18]==109 && digest[19]==184 && digest[20]==19 && digest[21]==176 && digest[22]==169 && digest[23]==100 && digest[24]==61 && digest[25]==70 && digest[26]==154 && digest[27]==47 && digest[28]==10 && digest[29]==107 && digest[30]==55 && digest[31]==242)
                {
                    return true;
                }
            }
            else if(digest[0]==58 && digest[1]==36 && digest[2]==152 && digest[3]==68 && digest[4]==51 && digest[5]==89 && digest[6]==1 && digest[7]==3 && digest[8]==206 && digest[9]==52 && digest[10]==8 && digest[11]==90 && digest[12]==21 && digest[13]==123 && digest[14]==244 && digest[15]==33 && digest[16]==125 && digest[17]==23 && digest[18]==113 && digest[19]==11 && digest[20]==239 && digest[21]==49 && digest[22]==3 && digest[23]==14 && digest[24]==107 && digest[25]==58 && digest[26]==208 && digest[27]==182 && digest[28]==87 && digest[29]==86 && digest[30]==166 && digest[31]==131)
            {
                return true;
            }
            else if(digest[0]==69 && digest[1]==13 && digest[2]==223 && digest[3]==22 && digest[4]==4 && digest[5]==217 && digest[6]==210 && digest[7]==145 && digest[8]==59 && digest[9]==59 && digest[10]==174 && digest[11]==193 && digest[12]==210 && digest[13]==185 && digest[14]==195 && digest[15]==137 && digest[16]==172 && digest[17]==81 && digest[18]==209 && digest[19]==131 && digest[20]==86 && digest[21]==104 && digest[22]==116 && digest[23]==202 && digest[24]==142 && digest[25]==241 && digest[26]==223 && digest[27]==213 && digest[28]==183 && digest[29]==159 && digest[30]==241 && digest[31]==80)
            {
                return true;
            }
            else if(digest[0]==75 && digest[1]==116 && digest[2]==199 && digest[3]==54 && digest[4]==31 && digest[5]==118 && digest[6]==33 && digest[7]==235 && digest[8]==69 && digest[9]==118 && digest[10]==148 && digest[11]==180 && digest[12]==181 && digest[13]==196 && digest[14]==226 && digest[15]==143 && digest[16]==121 && digest[17]==246 && digest[18]==183 && digest[19]==200 && digest[20]==213 && digest[21]==227 && digest[22]==16 && digest[23]==107 && digest[24]==95 && digest[25]==176 && digest[26]==0 && digest[27]==203 && digest[28]==157 && digest[29]==93 && digest[30]==86 && digest[31]==148)
            {
                return true;
            }
            else if(digest[0]==84 && digest[1]==78 && digest[2]==57 && digest[3]==129 && digest[4]==142 && digest[5]==131 && digest[6]==11 && digest[7]==20 && digest[8]==239 && digest[9]==121 && digest[10]==91 && digest[11]==237 && digest[12]==29 && digest[13]==37 && digest[14]==177 && digest[15]==2 && digest[16]==231 && digest[17]==20 && digest[18]==119 && digest[19]==157 && digest[20]==181 && digest[21]==92 && digest[22]==198 && digest[23]==229 && digest[24]==93 && digest[25]==26 && digest[26]==213 && digest[27]==147 && digest[28]==117 && digest[29]==25 && digest[30]==127 && digest[31]==116)
            {
                return true;
            }
            else if(digest[0]==86 && digest[1]==213 && digest[2]==54 && digest[3]==107 && digest[4]==224 && digest[5]==246 && digest[6]==220 && digest[7]==237 && digest[8]==60 && digest[9]==138 && digest[10]==27 && digest[11]==56 && digest[12]==162 && digest[13]==208 && digest[14]==44 && digest[15]==189 && digest[16]==32 && digest[17]==217 && digest[18]==232 && digest[19]==196 && digest[20]==79 && digest[21]==247 && digest[22]==77 && digest[23]==85 && digest[24]==243 && digest[25]==158 && digest[26]==31 && digest[27]==16 && digest[28]==179 && digest[29]==15 && digest[30]==214 && digest[31]==40)
            {
                return true;
            }
            else if(digest[0]==89 && digest[1]==35 && digest[2]==220 && digest[3]==177 && digest[4]==240 && digest[5]==130 && digest[6]==71 && digest[7]==123 && digest[8]==87 && digest[9]==73 && digest[10]==194 && digest[11]==162 && digest[12]==163 && digest[13]==179 && digest[14]==77 && digest[15]==161 && digest[16]==156 && digest[17]==49 && digest[18]==151 && digest[19]==225 && digest[20]==72 && digest[21]==179 && digest[22]==121 && digest[23]==30 && digest[24]==125 && digest[25]==166 && digest[26]==85 && digest[27]==40 && digest[28]==117 && digest[29]==138 && digest[30]==207 && digest[31]==140)
            {
                return true;
            }
            else if(digest[0]==93 && digest[1]==36 && digest[2]==66 && digest[3]==231 && digest[4]==116 && digest[5]==146 && digest[6]==183 && digest[7]==201 && digest[8]==39 && digest[9]==250 && digest[10]==143 && digest[11]==151 && digest[12]==93 && digest[13]==37 && digest[14]==87 && digest[15]==228 && digest[16]==66 && digest[17]==7 && digest[18]==28 && digest[19]==126 && digest[20]==141 && digest[21]==87 && digest[22]==9 && digest[23]==231 && digest[24]==171 && digest[25]==224 && digest[26]==238 && digest[27]==31 && digest[28]==16 && digest[29]==99 && digest[30]==45 && digest[31]==17)
            {
                return true;
            }

            return false;
        }
    }
}
