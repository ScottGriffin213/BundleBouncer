using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BundleBouncer.Data
{
    public class AvatarShitList
    {
        // Loaded from UserData/BundleBouncer/Avatars.txt in BundleBouncer.OnApplicationStart()
        public static HashSet<string> UserShitList;

        public static bool IsCrasher(string avID)
        {
            // No fun allowed.
            avID = avID.ToLowerInvariant();

            if (avID.StartsWith("local:"))
            {
                // Local test avatar, bypasses rules.
                return false;
            }

            if (!avID.StartsWith("avtr_"))
            {
                // Handles kaj's weird invalid bullshit.
                // Will probably also trap some very old avs but IDGAF.
                // FIXME: Whitelist any needed avIDs.
                return true;
            }

            // avIDs of crashers hashed so people can't just pull them from the binary.
            byte[] avhash;
            using (SHA256 hash = SHA256Managed.Create())
            {
                avhash = hash.ComputeHash(Encoding.UTF8.GetBytes(avID));
            }

            // The following is a bunch of generated if-trees created by putting
            // a bunch of avID SHA256s into a trie (https://en.wikipedia.org/wiki/Trie) and optimizing it.
            // Why?  Because I wanted to. Also offers some level of obfuscation.
            // Mostly the cool factor, though.
            if(avhash[0]==100 && avhash[1]==147 && avhash[2]==108 && avhash[3]==93 && avhash[4]==106 && avhash[5]==104 && avhash[6]==243 && avhash[7]==91 && avhash[8]==253 && avhash[9]==127 && avhash[10]==35 && avhash[11]==188 && avhash[12]==104 && avhash[13]==72 && avhash[14]==129 && avhash[15]==121 && avhash[16]==140 && avhash[17]==240 && avhash[18]==250 && avhash[19]==11 && avhash[20]==148 && avhash[21]==162 && avhash[22]==23 && avhash[23]==132 && avhash[24]==158 && avhash[25]==36 && avhash[26]==11 && avhash[27]==111 && avhash[28]==131 && avhash[29]==236 && avhash[30]==96 && avhash[31]==107)
            {
                return true;
            }
            else if(avhash[0]==102 && avhash[1]==110 && avhash[2]==70 && avhash[3]==138 && avhash[4]==189 && avhash[5]==245 && avhash[6]==168 && avhash[7]==55 && avhash[8]==22 && avhash[9]==193 && avhash[10]==91 && avhash[11]==3 && avhash[12]==75 && avhash[13]==20 && avhash[14]==49 && avhash[15]==32 && avhash[16]==138 && avhash[17]==169 && avhash[18]==254 && avhash[19]==119 && avhash[20]==238 && avhash[21]==86 && avhash[22]==235 && avhash[23]==115 && avhash[24]==222 && avhash[25]==60 && avhash[26]==200 && avhash[27]==178 && avhash[28]==69 && avhash[29]==110 && avhash[30]==64 && avhash[31]==57)
            {
                return true;
            }
            else if(avhash[0]==103 && avhash[1]==185 && avhash[2]==131 && avhash[3]==240 && avhash[4]==9 && avhash[5]==179 && avhash[6]==236 && avhash[7]==6 && avhash[8]==249 && avhash[9]==140 && avhash[10]==39 && avhash[11]==7 && avhash[12]==232 && avhash[13]==242 && avhash[14]==243 && avhash[15]==76 && avhash[16]==211 && avhash[17]==93 && avhash[18]==95 && avhash[19]==61 && avhash[20]==150 && avhash[21]==27 && avhash[22]==134 && avhash[23]==113 && avhash[24]==84 && avhash[25]==249 && avhash[26]==95 && avhash[27]==193 && avhash[28]==149 && avhash[29]==80 && avhash[30]==14 && avhash[31]==66)
            {
                return true;
            }
            else if(avhash[0]==107 && avhash[1]==221 && avhash[2]==110 && avhash[3]==167 && avhash[4]==180 && avhash[5]==181 && avhash[6]==178 && avhash[7]==162 && avhash[8]==19 && avhash[9]==41 && avhash[10]==209 && avhash[11]==127 && avhash[12]==151 && avhash[13]==237 && avhash[14]==184 && avhash[15]==47 && avhash[16]==229 && avhash[17]==49 && avhash[18]==115 && avhash[19]==198 && avhash[20]==6 && avhash[21]==70 && avhash[22]==27 && avhash[23]==195 && avhash[24]==54 && avhash[25]==32 && avhash[26]==103 && avhash[27]==254 && avhash[28]==132 && avhash[29]==61 && avhash[30]==155 && avhash[31]==113)
            {
                return true;
            }
            else if(avhash[0]==113)
            {
                if(avhash[1]==201 && avhash[2]==159 && avhash[3]==219 && avhash[4]==255 && avhash[5]==60 && avhash[6]==196 && avhash[7]==143 && avhash[8]==4 && avhash[9]==237 && avhash[10]==202 && avhash[11]==46 && avhash[12]==115 && avhash[13]==138 && avhash[14]==17 && avhash[15]==154 && avhash[16]==155 && avhash[17]==8 && avhash[18]==20 && avhash[19]==0 && avhash[20]==184 && avhash[21]==85 && avhash[22]==12 && avhash[23]==244 && avhash[24]==96 && avhash[25]==118 && avhash[26]==217 && avhash[27]==99 && avhash[28]==157 && avhash[29]==253 && avhash[30]==41 && avhash[31]==53)
                {
                    return true;
                }
                else if(avhash[1]==45 && avhash[2]==201 && avhash[3]==227 && avhash[4]==29 && avhash[5]==13 && avhash[6]==245 && avhash[7]==109 && avhash[8]==136 && avhash[9]==65 && avhash[10]==201 && avhash[11]==203 && avhash[12]==24 && avhash[13]==104 && avhash[14]==189 && avhash[15]==59 && avhash[16]==212 && avhash[17]==113 && avhash[18]==51 && avhash[19]==68 && avhash[20]==1 && avhash[21]==80 && avhash[22]==202 && avhash[23]==49 && avhash[24]==67 && avhash[25]==84 && avhash[26]==170 && avhash[27]==84 && avhash[28]==238 && avhash[29]==237 && avhash[30]==51 && avhash[31]==83)
                {
                    return true;
                }
            }
            else if(avhash[0]==115 && avhash[1]==150 && avhash[2]==130 && avhash[3]==39 && avhash[4]==26 && avhash[5]==172 && avhash[6]==110 && avhash[7]==87 && avhash[8]==167 && avhash[9]==105 && avhash[10]==239 && avhash[11]==19 && avhash[12]==19 && avhash[13]==37 && avhash[14]==208 && avhash[15]==197 && avhash[16]==97 && avhash[17]==34 && avhash[18]==9 && avhash[19]==155 && avhash[20]==209 && avhash[21]==102 && avhash[22]==107 && avhash[23]==255 && avhash[24]==134 && avhash[25]==172 && avhash[26]==21 && avhash[27]==181 && avhash[28]==103 && avhash[29]==140 && avhash[30]==5 && avhash[31]==79)
            {
                return true;
            }
            else if(avhash[0]==117 && avhash[1]==82 && avhash[2]==19 && avhash[3]==51 && avhash[4]==177 && avhash[5]==167 && avhash[6]==86 && avhash[7]==20 && avhash[8]==213 && avhash[9]==220 && avhash[10]==81 && avhash[11]==30 && avhash[12]==232 && avhash[13]==37 && avhash[14]==226 && avhash[15]==43 && avhash[16]==91 && avhash[17]==104 && avhash[18]==91 && avhash[19]==247 && avhash[20]==147 && avhash[21]==63 && avhash[22]==172 && avhash[23]==191 && avhash[24]==198 && avhash[25]==162 && avhash[26]==109 && avhash[27]==9 && avhash[28]==115 && avhash[29]==159 && avhash[30]==27 && avhash[31]==48)
            {
                return true;
            }
            else if(avhash[0]==118 && avhash[1]==185 && avhash[2]==148 && avhash[3]==79 && avhash[4]==31 && avhash[5]==88 && avhash[6]==24 && avhash[7]==221 && avhash[8]==60 && avhash[9]==239 && avhash[10]==111 && avhash[11]==236 && avhash[12]==69 && avhash[13]==60 && avhash[14]==99 && avhash[15]==18 && avhash[16]==183 && avhash[17]==212 && avhash[18]==142 && avhash[19]==92 && avhash[20]==10 && avhash[21]==239 && avhash[22]==231 && avhash[23]==78 && avhash[24]==61 && avhash[25]==212 && avhash[26]==106 && avhash[27]==144 && avhash[28]==19 && avhash[29]==210 && avhash[30]==98 && avhash[31]==144)
            {
                return true;
            }
            else if(avhash[0]==121 && avhash[1]==161 && avhash[2]==41 && avhash[3]==143 && avhash[4]==101 && avhash[5]==34 && avhash[6]==13 && avhash[7]==163 && avhash[8]==38 && avhash[9]==149 && avhash[10]==34 && avhash[11]==209 && avhash[12]==204 && avhash[13]==236 && avhash[14]==37 && avhash[15]==7 && avhash[16]==22 && avhash[17]==137 && avhash[18]==79 && avhash[19]==118 && avhash[20]==86 && avhash[21]==92 && avhash[22]==56 && avhash[23]==153 && avhash[24]==190 && avhash[25]==25 && avhash[26]==11 && avhash[27]==41 && avhash[28]==91 && avhash[29]==26 && avhash[30]==91 && avhash[31]==32)
            {
                return true;
            }
            else if(avhash[0]==123 && avhash[1]==85 && avhash[2]==60 && avhash[3]==225 && avhash[4]==63 && avhash[5]==37 && avhash[6]==177 && avhash[7]==232 && avhash[8]==171 && avhash[9]==180 && avhash[10]==69 && avhash[11]==124 && avhash[12]==22 && avhash[13]==61 && avhash[14]==54 && avhash[15]==120 && avhash[16]==5 && avhash[17]==66 && avhash[18]==242 && avhash[19]==216 && avhash[20]==216 && avhash[21]==20 && avhash[22]==87 && avhash[23]==80 && avhash[24]==57 && avhash[25]==247 && avhash[26]==255 && avhash[27]==250 && avhash[28]==148 && avhash[29]==149 && avhash[30]==49 && avhash[31]==181)
            {
                return true;
            }
            else if(avhash[0]==124 && avhash[1]==157 && avhash[2]==10 && avhash[3]==145 && avhash[4]==58 && avhash[5]==185 && avhash[6]==104 && avhash[7]==169 && avhash[8]==82 && avhash[9]==183 && avhash[10]==158 && avhash[11]==104 && avhash[12]==107 && avhash[13]==156 && avhash[14]==6 && avhash[15]==25 && avhash[16]==104 && avhash[17]==67 && avhash[18]==83 && avhash[19]==53 && avhash[20]==11 && avhash[21]==43 && avhash[22]==203 && avhash[23]==19 && avhash[24]==196 && avhash[25]==136 && avhash[26]==30 && avhash[27]==127 && avhash[28]==210 && avhash[29]==240 && avhash[30]==237 && avhash[31]==199)
            {
                return true;
            }
            else if(avhash[0]==127 && avhash[1]==45 && avhash[2]==162 && avhash[3]==33 && avhash[4]==159 && avhash[5]==174 && avhash[6]==26 && avhash[7]==200 && avhash[8]==125 && avhash[9]==131 && avhash[10]==139 && avhash[11]==1 && avhash[12]==243 && avhash[13]==230 && avhash[14]==60 && avhash[15]==105 && avhash[16]==194 && avhash[17]==132 && avhash[18]==244 && avhash[19]==60 && avhash[20]==139 && avhash[21]==178 && avhash[22]==33 && avhash[23]==125 && avhash[24]==241 && avhash[25]==78 && avhash[26]==255 && avhash[27]==52 && avhash[28]==255 && avhash[29]==152 && avhash[30]==145 && avhash[31]==206)
            {
                return true;
            }
            else if(avhash[0]==128 && avhash[1]==193 && avhash[2]==62 && avhash[3]==55 && avhash[4]==110 && avhash[5]==255 && avhash[6]==139 && avhash[7]==165 && avhash[8]==60 && avhash[9]==94 && avhash[10]==37 && avhash[11]==183 && avhash[12]==60 && avhash[13]==235 && avhash[14]==244 && avhash[15]==76 && avhash[16]==209 && avhash[17]==149 && avhash[18]==242 && avhash[19]==223 && avhash[20]==129 && avhash[21]==68 && avhash[22]==170 && avhash[23]==61 && avhash[24]==50 && avhash[25]==139 && avhash[26]==166 && avhash[27]==108 && avhash[28]==228 && avhash[29]==240 && avhash[30]==68 && avhash[31]==10)
            {
                return true;
            }
            else if(avhash[0]==134 && avhash[1]==99 && avhash[2]==252 && avhash[3]==42 && avhash[4]==171 && avhash[5]==106 && avhash[6]==9 && avhash[7]==181 && avhash[8]==71 && avhash[9]==182 && avhash[10]==185 && avhash[11]==121 && avhash[12]==40 && avhash[13]==6 && avhash[14]==114 && avhash[15]==252 && avhash[16]==235 && avhash[17]==157 && avhash[18]==141 && avhash[19]==156 && avhash[20]==141 && avhash[21]==127 && avhash[22]==196 && avhash[23]==29 && avhash[24]==228 && avhash[25]==33 && avhash[26]==135 && avhash[27]==182 && avhash[28]==39 && avhash[29]==182 && avhash[30]==174 && avhash[31]==91)
            {
                return true;
            }
            else if(avhash[0]==135 && avhash[1]==93 && avhash[2]==197 && avhash[3]==237 && avhash[4]==244 && avhash[5]==235 && avhash[6]==210 && avhash[7]==170 && avhash[8]==183 && avhash[9]==171 && avhash[10]==185 && avhash[11]==29 && avhash[12]==175 && avhash[13]==240 && avhash[14]==181 && avhash[15]==225 && avhash[16]==101 && avhash[17]==47 && avhash[18]==47 && avhash[19]==75 && avhash[20]==117 && avhash[21]==198 && avhash[22]==101 && avhash[23]==161 && avhash[24]==136 && avhash[25]==139 && avhash[26]==182 && avhash[27]==252 && avhash[28]==150 && avhash[29]==132 && avhash[30]==243 && avhash[31]==169)
            {
                return true;
            }
            else if(avhash[0]==138)
            {
                if(avhash[1]==66 && avhash[2]==130 && avhash[3]==108 && avhash[4]==8 && avhash[5]==162 && avhash[6]==176 && avhash[7]==180 && avhash[8]==220 && avhash[9]==9 && avhash[10]==1 && avhash[11]==139 && avhash[12]==251 && avhash[13]==146 && avhash[14]==99 && avhash[15]==124 && avhash[16]==197 && avhash[17]==190 && avhash[18]==250 && avhash[19]==181 && avhash[20]==170 && avhash[21]==12 && avhash[22]==225 && avhash[23]==212 && avhash[24]==121 && avhash[25]==10 && avhash[26]==248 && avhash[27]==197 && avhash[28]==124 && avhash[29]==53 && avhash[30]==53 && avhash[31]==50)
                {
                    return true;
                }
                else if(avhash[1]==74 && avhash[2]==149 && avhash[3]==96 && avhash[4]==189 && avhash[5]==1 && avhash[6]==241 && avhash[7]==150 && avhash[8]==145 && avhash[9]==224 && avhash[10]==139 && avhash[11]==125 && avhash[12]==183 && avhash[13]==20 && avhash[14]==2 && avhash[15]==53 && avhash[16]==43 && avhash[17]==107 && avhash[18]==145 && avhash[19]==239 && avhash[20]==114 && avhash[21]==67 && avhash[22]==110 && avhash[23]==150 && avhash[24]==77 && avhash[25]==131 && avhash[26]==50 && avhash[27]==142 && avhash[28]==110 && avhash[29]==40 && avhash[30]==232 && avhash[31]==28)
                {
                    return true;
                }
            }
            else if(avhash[0]==139 && avhash[1]==191 && avhash[2]==22 && avhash[3]==85 && avhash[4]==208 && avhash[5]==10 && avhash[6]==141 && avhash[7]==123 && avhash[8]==24 && avhash[9]==62 && avhash[10]==201 && avhash[11]==117 && avhash[12]==186 && avhash[13]==114 && avhash[14]==82 && avhash[15]==212 && avhash[16]==166 && avhash[17]==188 && avhash[18]==72 && avhash[19]==140 && avhash[20]==180 && avhash[21]==173 && avhash[22]==172 && avhash[23]==63 && avhash[24]==188 && avhash[25]==49 && avhash[26]==195 && avhash[27]==80 && avhash[28]==53 && avhash[29]==223 && avhash[30]==254 && avhash[31]==108)
            {
                return true;
            }
            else if(avhash[0]==141 && avhash[1]==120 && avhash[2]==133 && avhash[3]==51 && avhash[4]==91 && avhash[5]==9 && avhash[6]==57 && avhash[7]==173 && avhash[8]==10 && avhash[9]==240 && avhash[10]==233 && avhash[11]==177 && avhash[12]==59 && avhash[13]==215 && avhash[14]==131 && avhash[15]==75 && avhash[16]==65 && avhash[17]==190 && avhash[18]==172 && avhash[19]==154 && avhash[20]==194 && avhash[21]==154 && avhash[22]==194 && avhash[23]==148 && avhash[24]==190 && avhash[25]==38 && avhash[26]==76 && avhash[27]==152 && avhash[28]==103 && avhash[29]==174 && avhash[30]==217 && avhash[31]==95)
            {
                return true;
            }
            else if(avhash[0]==142 && avhash[1]==176 && avhash[2]==130 && avhash[3]==101 && avhash[4]==142 && avhash[5]==161 && avhash[6]==56 && avhash[7]==62 && avhash[8]==28 && avhash[9]==241 && avhash[10]==205 && avhash[11]==165 && avhash[12]==56 && avhash[13]==173 && avhash[14]==117 && avhash[15]==222 && avhash[16]==107 && avhash[17]==201 && avhash[18]==161 && avhash[19]==56 && avhash[20]==81 && avhash[21]==108 && avhash[22]==191 && avhash[23]==121 && avhash[24]==159 && avhash[25]==18 && avhash[26]==124 && avhash[27]==116 && avhash[28]==201 && avhash[29]==144 && avhash[30]==93 && avhash[31]==213)
            {
                return true;
            }
            else if(avhash[0]==143 && avhash[1]==69 && avhash[2]==93 && avhash[3]==114 && avhash[4]==247 && avhash[5]==110 && avhash[6]==238 && avhash[7]==17 && avhash[8]==199 && avhash[9]==33 && avhash[10]==205 && avhash[11]==255 && avhash[12]==128 && avhash[13]==128 && avhash[14]==96 && avhash[15]==127 && avhash[16]==206 && avhash[17]==109 && avhash[18]==86 && avhash[19]==243 && avhash[20]==210 && avhash[21]==24 && avhash[22]==3 && avhash[23]==249 && avhash[24]==189 && avhash[25]==10 && avhash[26]==241 && avhash[27]==255 && avhash[28]==29 && avhash[29]==166 && avhash[30]==13 && avhash[31]==112)
            {
                return true;
            }
            else if(avhash[0]==15 && avhash[1]==166 && avhash[2]==55 && avhash[3]==128 && avhash[4]==2 && avhash[5]==242 && avhash[6]==227 && avhash[7]==95 && avhash[8]==40 && avhash[9]==15 && avhash[10]==106 && avhash[11]==76 && avhash[12]==92 && avhash[13]==63 && avhash[14]==21 && avhash[15]==248 && avhash[16]==228 && avhash[17]==236 && avhash[18]==203 && avhash[19]==215 && avhash[20]==92 && avhash[21]==199 && avhash[22]==211 && avhash[23]==208 && avhash[24]==220 && avhash[25]==201 && avhash[26]==33 && avhash[27]==116 && avhash[28]==114 && avhash[29]==36 && avhash[30]==2 && avhash[31]==9)
            {
                return true;
            }
            else if(avhash[0]==150 && avhash[1]==221 && avhash[2]==114 && avhash[3]==8 && avhash[4]==85 && avhash[5]==135 && avhash[6]==158 && avhash[7]==73 && avhash[8]==240 && avhash[9]==201 && avhash[10]==0 && avhash[11]==96 && avhash[12]==112 && avhash[13]==205 && avhash[14]==180 && avhash[15]==163 && avhash[16]==139 && avhash[17]==163 && avhash[18]==73 && avhash[19]==145 && avhash[20]==219 && avhash[21]==171 && avhash[22]==89 && avhash[23]==31 && avhash[24]==143 && avhash[25]==123 && avhash[26]==92 && avhash[27]==194 && avhash[28]==232 && avhash[29]==184 && avhash[30]==209 && avhash[31]==91)
            {
                return true;
            }
            else if(avhash[0]==152 && avhash[1]==11 && avhash[2]==153 && avhash[3]==228 && avhash[4]==60 && avhash[5]==138 && avhash[6]==118 && avhash[7]==223 && avhash[8]==3 && avhash[9]==24 && avhash[10]==116 && avhash[11]==55 && avhash[12]==80 && avhash[13]==36 && avhash[14]==172 && avhash[15]==198 && avhash[16]==123 && avhash[17]==137 && avhash[18]==195 && avhash[19]==244 && avhash[20]==121 && avhash[21]==239 && avhash[22]==97 && avhash[23]==216 && avhash[24]==95 && avhash[25]==125 && avhash[26]==168 && avhash[27]==238 && avhash[28]==52 && avhash[29]==85 && avhash[30]==186 && avhash[31]==1)
            {
                return true;
            }
            else if(avhash[0]==153)
            {
                if(avhash[1]==102 && avhash[2]==25 && avhash[3]==155 && avhash[4]==252 && avhash[5]==142 && avhash[6]==99 && avhash[7]==159 && avhash[8]==116 && avhash[9]==157 && avhash[10]==190 && avhash[11]==180 && avhash[12]==233 && avhash[13]==79 && avhash[14]==43 && avhash[15]==128 && avhash[16]==187 && avhash[17]==34 && avhash[18]==225 && avhash[19]==254 && avhash[20]==252 && avhash[21]==94 && avhash[22]==211 && avhash[23]==137 && avhash[24]==234 && avhash[25]==198 && avhash[26]==83 && avhash[27]==156 && avhash[28]==205 && avhash[29]==31 && avhash[30]==106 && avhash[31]==239)
                {
                    return true;
                }
                else if(avhash[1]==107 && avhash[2]==58 && avhash[3]==134 && avhash[4]==244 && avhash[5]==162 && avhash[6]==226 && avhash[7]==95 && avhash[8]==100 && avhash[9]==24 && avhash[10]==27 && avhash[11]==109 && avhash[12]==243 && avhash[13]==66 && avhash[14]==43 && avhash[15]==98 && avhash[16]==229 && avhash[17]==154 && avhash[18]==1 && avhash[19]==216 && avhash[20]==64 && avhash[21]==143 && avhash[22]==168 && avhash[23]==195 && avhash[24]==238 && avhash[25]==239 && avhash[26]==187 && avhash[27]==201 && avhash[28]==161 && avhash[29]==110 && avhash[30]==229 && avhash[31]==30)
                {
                    return true;
                }
            }
            else if(avhash[0]==154)
            {
                if(avhash[1]==31 && avhash[2]==163 && avhash[3]==66 && avhash[4]==127 && avhash[5]==20 && avhash[6]==149 && avhash[7]==152 && avhash[8]==190 && avhash[9]==16 && avhash[10]==9 && avhash[11]==159 && avhash[12]==53 && avhash[13]==149 && avhash[14]==10 && avhash[15]==254 && avhash[16]==12 && avhash[17]==214 && avhash[18]==224 && avhash[19]==111 && avhash[20]==149 && avhash[21]==38 && avhash[22]==175 && avhash[23]==248 && avhash[24]==176 && avhash[25]==113 && avhash[26]==218 && avhash[27]==241 && avhash[28]==55 && avhash[29]==185 && avhash[30]==132 && avhash[31]==151)
                {
                    return true;
                }
                else if(avhash[1]==66 && avhash[2]==241 && avhash[3]==135 && avhash[4]==182 && avhash[5]==31 && avhash[6]==132 && avhash[7]==99 && avhash[8]==215 && avhash[9]==179 && avhash[10]==203 && avhash[11]==209 && avhash[12]==91 && avhash[13]==11 && avhash[14]==44 && avhash[15]==7 && avhash[16]==46 && avhash[17]==247 && avhash[18]==190 && avhash[19]==192 && avhash[20]==136 && avhash[21]==35 && avhash[22]==131 && avhash[23]==106 && avhash[24]==175 && avhash[25]==148 && avhash[26]==103 && avhash[27]==185 && avhash[28]==59 && avhash[29]==190 && avhash[30]==8 && avhash[31]==168)
                {
                    return true;
                }
            }
            else if(avhash[0]==157 && avhash[1]==245 && avhash[2]==148 && avhash[3]==214 && avhash[4]==125 && avhash[5]==13 && avhash[6]==18 && avhash[7]==189 && avhash[8]==245 && avhash[9]==80 && avhash[10]==208 && avhash[11]==17 && avhash[12]==11 && avhash[13]==102 && avhash[14]==146 && avhash[15]==82 && avhash[16]==201 && avhash[17]==195 && avhash[18]==190 && avhash[19]==76 && avhash[20]==32 && avhash[21]==197 && avhash[22]==17 && avhash[23]==172 && avhash[24]==112 && avhash[25]==85 && avhash[26]==72 && avhash[27]==115 && avhash[28]==115 && avhash[29]==30 && avhash[30]==110 && avhash[31]==134)
            {
                return true;
            }
            else if(avhash[0]==158 && avhash[1]==207 && avhash[2]==132 && avhash[3]==186 && avhash[4]==113 && avhash[5]==68 && avhash[6]==91 && avhash[7]==238 && avhash[8]==37 && avhash[9]==224 && avhash[10]==18 && avhash[11]==43 && avhash[12]==76 && avhash[13]==156 && avhash[14]==7 && avhash[15]==173 && avhash[16]==179 && avhash[17]==189 && avhash[18]==158 && avhash[19]==102 && avhash[20]==121 && avhash[21]==126 && avhash[22]==50 && avhash[23]==136 && avhash[24]==104 && avhash[25]==66 && avhash[26]==237 && avhash[27]==186 && avhash[28]==224 && avhash[29]==230 && avhash[30]==137 && avhash[31]==54)
            {
                return true;
            }
            else if(avhash[0]==16 && avhash[1]==231 && avhash[2]==145 && avhash[3]==130 && avhash[4]==45 && avhash[5]==73 && avhash[6]==48 && avhash[7]==15 && avhash[8]==89 && avhash[9]==66 && avhash[10]==200 && avhash[11]==201 && avhash[12]==173 && avhash[13]==84 && avhash[14]==92 && avhash[15]==175 && avhash[16]==120 && avhash[17]==225 && avhash[18]==22 && avhash[19]==124 && avhash[20]==223 && avhash[21]==18 && avhash[22]==213 && avhash[23]==251 && avhash[24]==37 && avhash[25]==90 && avhash[26]==163 && avhash[27]==32 && avhash[28]==106 && avhash[29]==57 && avhash[30]==132 && avhash[31]==145)
            {
                return true;
            }
            else if(avhash[0]==164 && avhash[1]==54 && avhash[2]==201 && avhash[3]==4 && avhash[4]==23 && avhash[5]==206 && avhash[6]==147 && avhash[7]==69 && avhash[8]==8 && avhash[9]==216 && avhash[10]==149 && avhash[11]==225 && avhash[12]==140 && avhash[13]==135 && avhash[14]==97 && avhash[15]==25 && avhash[16]==50 && avhash[17]==67 && avhash[18]==241 && avhash[19]==199 && avhash[20]==60 && avhash[21]==182 && avhash[22]==75 && avhash[23]==139 && avhash[24]==193 && avhash[25]==86 && avhash[26]==185 && avhash[27]==178 && avhash[28]==238 && avhash[29]==52 && avhash[30]==38 && avhash[31]==65)
            {
                return true;
            }
            else if(avhash[0]==167)
            {
                if(avhash[1]==104 && avhash[2]==61 && avhash[3]==153 && avhash[4]==168 && avhash[5]==65 && avhash[6]==212 && avhash[7]==245 && avhash[8]==125 && avhash[9]==17 && avhash[10]==227 && avhash[11]==126 && avhash[12]==43 && avhash[13]==0 && avhash[14]==64 && avhash[15]==58 && avhash[16]==205 && avhash[17]==154 && avhash[18]==236 && avhash[19]==173 && avhash[20]==16 && avhash[21]==6 && avhash[22]==79 && avhash[23]==228 && avhash[24]==213 && avhash[25]==190 && avhash[26]==102 && avhash[27]==110 && avhash[28]==241 && avhash[29]==236 && avhash[30]==125 && avhash[31]==64)
                {
                    return true;
                }
                else if(avhash[1]==143 && avhash[2]==47 && avhash[3]==230 && avhash[4]==133 && avhash[5]==218 && avhash[6]==181 && avhash[7]==210 && avhash[8]==33 && avhash[9]==42 && avhash[10]==38 && avhash[11]==252 && avhash[12]==72 && avhash[13]==50 && avhash[14]==176 && avhash[15]==233 && avhash[16]==195 && avhash[17]==255 && avhash[18]==145 && avhash[19]==66 && avhash[20]==221 && avhash[21]==184 && avhash[22]==178 && avhash[23]==158 && avhash[24]==247 && avhash[25]==215 && avhash[26]==35 && avhash[27]==229 && avhash[28]==156 && avhash[29]==75 && avhash[30]==82 && avhash[31]==126)
                {
                    return true;
                }
                else if(avhash[1]==5 && avhash[2]==217 && avhash[3]==220 && avhash[4]==19 && avhash[5]==46 && avhash[6]==205 && avhash[7]==63 && avhash[8]==234 && avhash[9]==70 && avhash[10]==156 && avhash[11]==48 && avhash[12]==112 && avhash[13]==119 && avhash[14]==151 && avhash[15]==60 && avhash[16]==187 && avhash[17]==57 && avhash[18]==56 && avhash[19]==9 && avhash[20]==158 && avhash[21]==211 && avhash[22]==186 && avhash[23]==197 && avhash[24]==175 && avhash[25]==177 && avhash[26]==47 && avhash[27]==191 && avhash[28]==5 && avhash[29]==125 && avhash[30]==227 && avhash[31]==213)
                {
                    return true;
                }
            }
            else if(avhash[0]==17 && avhash[1]==80 && avhash[2]==119 && avhash[3]==101 && avhash[4]==41 && avhash[5]==170 && avhash[6]==215 && avhash[7]==111 && avhash[8]==4 && avhash[9]==207 && avhash[10]==150 && avhash[11]==210 && avhash[12]==66 && avhash[13]==209 && avhash[14]==240 && avhash[15]==137 && avhash[16]==85 && avhash[17]==52 && avhash[18]==54 && avhash[19]==133 && avhash[20]==121 && avhash[21]==235 && avhash[22]==223 && avhash[23]==165 && avhash[24]==218 && avhash[25]==74 && avhash[26]==51 && avhash[27]==209 && avhash[28]==99 && avhash[29]==205 && avhash[30]==86 && avhash[31]==62)
            {
                return true;
            }
            else if(avhash[0]==173 && avhash[1]==180 && avhash[2]==84 && avhash[3]==32 && avhash[4]==88 && avhash[5]==109 && avhash[6]==39 && avhash[7]==161 && avhash[8]==243 && avhash[9]==151 && avhash[10]==255 && avhash[11]==229 && avhash[12]==109 && avhash[13]==148 && avhash[14]==35 && avhash[15]==109 && avhash[16]==137 && avhash[17]==222 && avhash[18]==108 && avhash[19]==18 && avhash[20]==187 && avhash[21]==162 && avhash[22]==10 && avhash[23]==157 && avhash[24]==81 && avhash[25]==182 && avhash[26]==63 && avhash[27]==192 && avhash[28]==221 && avhash[29]==163 && avhash[30]==176 && avhash[31]==213)
            {
                return true;
            }
            else if(avhash[0]==174 && avhash[1]==132 && avhash[2]==85 && avhash[3]==97 && avhash[4]==197 && avhash[5]==164 && avhash[6]==75 && avhash[7]==78 && avhash[8]==63 && avhash[9]==253 && avhash[10]==120 && avhash[11]==180 && avhash[12]==255 && avhash[13]==110 && avhash[14]==145 && avhash[15]==164 && avhash[16]==52 && avhash[17]==45 && avhash[18]==80 && avhash[19]==224 && avhash[20]==143 && avhash[21]==17 && avhash[22]==0 && avhash[23]==72 && avhash[24]==138 && avhash[25]==52 && avhash[26]==247 && avhash[27]==68 && avhash[28]==46 && avhash[29]==6 && avhash[30]==133 && avhash[31]==254)
            {
                return true;
            }
            else if(avhash[0]==176 && avhash[1]==128 && avhash[2]==62 && avhash[3]==177 && avhash[4]==207 && avhash[5]==41 && avhash[6]==229 && avhash[7]==132 && avhash[8]==77 && avhash[9]==216 && avhash[10]==173 && avhash[11]==193 && avhash[12]==140 && avhash[13]==252 && avhash[14]==190 && avhash[15]==85 && avhash[16]==112 && avhash[17]==154 && avhash[18]==8 && avhash[19]==153 && avhash[20]==119 && avhash[21]==206 && avhash[22]==183 && avhash[23]==38 && avhash[24]==46 && avhash[25]==36 && avhash[26]==194 && avhash[27]==249 && avhash[28]==134 && avhash[29]==216 && avhash[30]==152 && avhash[31]==251)
            {
                return true;
            }
            else if(avhash[0]==177)
            {
                if(avhash[1]==218 && avhash[2]==83 && avhash[3]==222 && avhash[4]==22 && avhash[5]==27 && avhash[6]==202 && avhash[7]==186 && avhash[8]==235 && avhash[9]==192 && avhash[10]==101 && avhash[11]==134 && avhash[12]==231 && avhash[13]==114 && avhash[14]==23 && avhash[15]==159 && avhash[16]==56 && avhash[17]==62 && avhash[18]==104 && avhash[19]==99 && avhash[20]==129 && avhash[21]==78 && avhash[22]==107 && avhash[23]==42 && avhash[24]==193 && avhash[25]==103 && avhash[26]==107 && avhash[27]==142 && avhash[28]==128 && avhash[29]==173 && avhash[30]==29 && avhash[31]==117)
                {
                    return true;
                }
                else if(avhash[1]==37 && avhash[2]==221 && avhash[3]==95 && avhash[4]==55 && avhash[5]==97 && avhash[6]==144 && avhash[7]==117 && avhash[8]==180 && avhash[9]==132 && avhash[10]==92 && avhash[11]==17 && avhash[12]==142 && avhash[13]==173 && avhash[14]==127 && avhash[15]==248 && avhash[16]==156 && avhash[17]==15 && avhash[18]==235 && avhash[19]==151 && avhash[20]==46 && avhash[21]==147 && avhash[22]==50 && avhash[23]==103 && avhash[24]==228 && avhash[25]==51 && avhash[26]==254 && avhash[27]==101 && avhash[28]==162 && avhash[29]==102 && avhash[30]==89 && avhash[31]==206)
                {
                    return true;
                }
            }
            else if(avhash[0]==183 && avhash[1]==48 && avhash[2]==109 && avhash[3]==56 && avhash[4]==149 && avhash[5]==184 && avhash[6]==24 && avhash[7]==119 && avhash[8]==161 && avhash[9]==90 && avhash[10]==166 && avhash[11]==166 && avhash[12]==93 && avhash[13]==16 && avhash[14]==250 && avhash[15]==35 && avhash[16]==210 && avhash[17]==0 && avhash[18]==123 && avhash[19]==69 && avhash[20]==140 && avhash[21]==168 && avhash[22]==132 && avhash[23]==241 && avhash[24]==119 && avhash[25]==25 && avhash[26]==246 && avhash[27]==224 && avhash[28]==61 && avhash[29]==108 && avhash[30]==231 && avhash[31]==204)
            {
                return true;
            }
            else if(avhash[0]==185 && avhash[1]==31 && avhash[2]==203 && avhash[3]==133 && avhash[4]==81 && avhash[5]==222 && avhash[6]==49 && avhash[7]==106 && avhash[8]==43 && avhash[9]==41 && avhash[10]==12 && avhash[11]==119 && avhash[12]==242 && avhash[13]==206 && avhash[14]==3 && avhash[15]==107 && avhash[16]==195 && avhash[17]==255 && avhash[18]==254 && avhash[19]==255 && avhash[20]==82 && avhash[21]==30 && avhash[22]==55 && avhash[23]==218 && avhash[24]==237 && avhash[25]==226 && avhash[26]==120 && avhash[27]==68 && avhash[28]==13 && avhash[29]==43 && avhash[30]==89 && avhash[31]==146)
            {
                return true;
            }
            else if(avhash[0]==195 && avhash[1]==43 && avhash[2]==252 && avhash[3]==11 && avhash[4]==22 && avhash[5]==201 && avhash[6]==112 && avhash[7]==218 && avhash[8]==131 && avhash[9]==122 && avhash[10]==41 && avhash[11]==103 && avhash[12]==112 && avhash[13]==238 && avhash[14]==220 && avhash[15]==25 && avhash[16]==50 && avhash[17]==137 && avhash[18]==47 && avhash[19]==26 && avhash[20]==103 && avhash[21]==59 && avhash[22]==3 && avhash[23]==232 && avhash[24]==61 && avhash[25]==77 && avhash[26]==83 && avhash[27]==103 && avhash[28]==242 && avhash[29]==78 && avhash[30]==10 && avhash[31]==220)
            {
                return true;
            }
            else if(avhash[0]==196)
            {
                if(avhash[1]==157 && avhash[2]==59 && avhash[3]==86 && avhash[4]==236 && avhash[5]==227 && avhash[6]==97 && avhash[7]==210 && avhash[8]==75 && avhash[9]==28 && avhash[10]==88 && avhash[11]==87 && avhash[12]==87 && avhash[13]==2 && avhash[14]==215 && avhash[15]==242 && avhash[16]==89 && avhash[17]==255 && avhash[18]==149 && avhash[19]==81 && avhash[20]==9 && avhash[21]==153 && avhash[22]==144 && avhash[23]==116 && avhash[24]==134 && avhash[25]==242 && avhash[26]==163 && avhash[27]==33 && avhash[28]==144 && avhash[29]==231 && avhash[30]==44 && avhash[31]==216)
                {
                    return true;
                }
                else if(avhash[1]==55 && avhash[2]==64 && avhash[3]==113 && avhash[4]==147 && avhash[5]==87 && avhash[6]==32 && avhash[7]==29 && avhash[8]==141 && avhash[9]==118 && avhash[10]==16 && avhash[11]==121 && avhash[12]==134 && avhash[13]==65 && avhash[14]==238 && avhash[15]==13 && avhash[16]==124 && avhash[17]==45 && avhash[18]==91 && avhash[19]==117 && avhash[20]==163 && avhash[21]==204 && avhash[22]==69 && avhash[23]==132 && avhash[24]==74 && avhash[25]==177 && avhash[26]==7 && avhash[27]==142 && avhash[28]==128 && avhash[29]==114 && avhash[30]==4 && avhash[31]==88)
                {
                    return true;
                }
            }
            else if(avhash[0]==197)
            {
                if(avhash[1]==160 && avhash[2]==37 && avhash[3]==23 && avhash[4]==138 && avhash[5]==230 && avhash[6]==102 && avhash[7]==141 && avhash[8]==202 && avhash[9]==216 && avhash[10]==173 && avhash[11]==212 && avhash[12]==74 && avhash[13]==118 && avhash[14]==169 && avhash[15]==157 && avhash[16]==79 && avhash[17]==182 && avhash[18]==158 && avhash[19]==144 && avhash[20]==86 && avhash[21]==15 && avhash[22]==10 && avhash[23]==132 && avhash[24]==141 && avhash[25]==144 && avhash[26]==238 && avhash[27]==34 && avhash[28]==170 && avhash[29]==195 && avhash[30]==213 && avhash[31]==244)
                {
                    return true;
                }
                else if(avhash[1]==88 && avhash[2]==241 && avhash[3]==121 && avhash[4]==16 && avhash[5]==173 && avhash[6]==33 && avhash[7]==208 && avhash[8]==74 && avhash[9]==25 && avhash[10]==228 && avhash[11]==165 && avhash[12]==222 && avhash[13]==63 && avhash[14]==192 && avhash[15]==37 && avhash[16]==151 && avhash[17]==60 && avhash[18]==140 && avhash[19]==205 && avhash[20]==209 && avhash[21]==11 && avhash[22]==51 && avhash[23]==177 && avhash[24]==165 && avhash[25]==235 && avhash[26]==195 && avhash[27]==101 && avhash[28]==16 && avhash[29]==231 && avhash[30]==150 && avhash[31]==8)
                {
                    return true;
                }
            }
            else if(avhash[0]==199 && avhash[1]==107 && avhash[2]==27 && avhash[3]==68 && avhash[4]==89 && avhash[5]==192 && avhash[6]==188 && avhash[7]==64 && avhash[8]==187 && avhash[9]==218 && avhash[10]==182 && avhash[11]==46 && avhash[12]==199 && avhash[13]==176 && avhash[14]==10 && avhash[15]==240 && avhash[16]==184 && avhash[17]==249 && avhash[18]==179 && avhash[19]==13 && avhash[20]==140 && avhash[21]==161 && avhash[22]==140 && avhash[23]==31 && avhash[24]==111 && avhash[25]==112 && avhash[26]==32 && avhash[27]==43 && avhash[28]==45 && avhash[29]==11 && avhash[30]==129 && avhash[31]==100)
            {
                return true;
            }
            else if(avhash[0]==2)
            {
                if(avhash[1]==114 && avhash[2]==174 && avhash[3]==4 && avhash[4]==132 && avhash[5]==171 && avhash[6]==213 && avhash[7]==137 && avhash[8]==49 && avhash[9]==140 && avhash[10]==253 && avhash[11]==89 && avhash[12]==38 && avhash[13]==168 && avhash[14]==85 && avhash[15]==162 && avhash[16]==124 && avhash[17]==113 && avhash[18]==93 && avhash[19]==22 && avhash[20]==128 && avhash[21]==130 && avhash[22]==242 && avhash[23]==136 && avhash[24]==244 && avhash[25]==189 && avhash[26]==100 && avhash[27]==153 && avhash[28]==75 && avhash[29]==88 && avhash[30]==223 && avhash[31]==6)
                {
                    return true;
                }
                else if(avhash[1]==215 && avhash[2]==19 && avhash[3]==12 && avhash[4]==142 && avhash[5]==80 && avhash[6]==43 && avhash[7]==131 && avhash[8]==95 && avhash[9]==71 && avhash[10]==190 && avhash[11]==208 && avhash[12]==227 && avhash[13]==16 && avhash[14]==249 && avhash[15]==142 && avhash[16]==250 && avhash[17]==64 && avhash[18]==165 && avhash[19]==150 && avhash[20]==94 && avhash[21]==58 && avhash[22]==157 && avhash[23]==211 && avhash[24]==233 && avhash[25]==26 && avhash[26]==144 && avhash[27]==208 && avhash[28]==247 && avhash[29]==81 && avhash[30]==40 && avhash[31]==4)
                {
                    return true;
                }
            }
            else if(avhash[0]==200 && avhash[1]==130 && avhash[2]==54 && avhash[3]==95 && avhash[4]==216 && avhash[5]==251 && avhash[6]==59 && avhash[7]==205 && avhash[8]==110 && avhash[9]==241 && avhash[10]==62 && avhash[11]==37 && avhash[12]==186 && avhash[13]==33 && avhash[14]==238 && avhash[15]==181 && avhash[16]==142 && avhash[17]==153 && avhash[18]==228 && avhash[19]==36 && avhash[20]==87 && avhash[21]==80 && avhash[22]==247 && avhash[23]==66 && avhash[24]==160 && avhash[25]==158 && avhash[26]==171 && avhash[27]==95 && avhash[28]==202 && avhash[29]==218 && avhash[30]==206 && avhash[31]==124)
            {
                return true;
            }
            else if(avhash[0]==202)
            {
                if(avhash[1]==153 && avhash[2]==255 && avhash[3]==144 && avhash[4]==232 && avhash[5]==77 && avhash[6]==139 && avhash[7]==166 && avhash[8]==244 && avhash[9]==82 && avhash[10]==56 && avhash[11]==64 && avhash[12]==105 && avhash[13]==5 && avhash[14]==54 && avhash[15]==56 && avhash[16]==160 && avhash[17]==229 && avhash[18]==242 && avhash[19]==160 && avhash[20]==27 && avhash[21]==28 && avhash[22]==98 && avhash[23]==21 && avhash[24]==21 && avhash[25]==162 && avhash[26]==54 && avhash[27]==235 && avhash[28]==255 && avhash[29]==22 && avhash[30]==222 && avhash[31]==223)
                {
                    return true;
                }
                else if(avhash[1]==155 && avhash[2]==36 && avhash[3]==14 && avhash[4]==207 && avhash[5]==122 && avhash[6]==249 && avhash[7]==112 && avhash[8]==14 && avhash[9]==14 && avhash[10]==181 && avhash[11]==103 && avhash[12]==162 && avhash[13]==158 && avhash[14]==93 && avhash[15]==135 && avhash[16]==6 && avhash[17]==142 && avhash[18]==173 && avhash[19]==197 && avhash[20]==49 && avhash[21]==211 && avhash[22]==235 && avhash[23]==95 && avhash[24]==122 && avhash[25]==33 && avhash[26]==81 && avhash[27]==10 && avhash[28]==235 && avhash[29]==87 && avhash[30]==120 && avhash[31]==44)
                {
                    return true;
                }
            }
            else if(avhash[0]==205 && avhash[1]==54 && avhash[2]==11 && avhash[3]==228 && avhash[4]==237 && avhash[5]==174 && avhash[6]==211 && avhash[7]==226 && avhash[8]==219 && avhash[9]==46 && avhash[10]==13 && avhash[11]==53 && avhash[12]==103 && avhash[13]==54 && avhash[14]==154 && avhash[15]==187 && avhash[16]==164 && avhash[17]==159 && avhash[18]==148 && avhash[19]==109 && avhash[20]==163 && avhash[21]==184 && avhash[22]==2 && avhash[23]==243 && avhash[24]==170 && avhash[25]==240 && avhash[26]==48 && avhash[27]==29 && avhash[28]==251 && avhash[29]==124 && avhash[30]==98 && avhash[31]==90)
            {
                return true;
            }
            else if(avhash[0]==207 && avhash[1]==167 && avhash[2]==119 && avhash[3]==13 && avhash[4]==108 && avhash[5]==161 && avhash[6]==252 && avhash[7]==208 && avhash[8]==175 && avhash[9]==148 && avhash[10]==182 && avhash[11]==119 && avhash[12]==117 && avhash[13]==201 && avhash[14]==210 && avhash[15]==101 && avhash[16]==52 && avhash[17]==38 && avhash[18]==174 && avhash[19]==107 && avhash[20]==100 && avhash[21]==96 && avhash[22]==141 && avhash[23]==166 && avhash[24]==98 && avhash[25]==187 && avhash[26]==125 && avhash[27]==28 && avhash[28]==230 && avhash[29]==80 && avhash[30]==194 && avhash[31]==79)
            {
                return true;
            }
            else if(avhash[0]==209 && avhash[1]==175 && avhash[2]==78 && avhash[3]==33 && avhash[4]==254 && avhash[5]==36 && avhash[6]==74 && avhash[7]==90 && avhash[8]==181 && avhash[9]==216 && avhash[10]==128 && avhash[11]==226 && avhash[12]==36 && avhash[13]==237 && avhash[14]==211 && avhash[15]==83 && avhash[16]==124 && avhash[17]==169 && avhash[18]==98 && avhash[19]==158 && avhash[20]==60 && avhash[21]==234 && avhash[22]==51 && avhash[23]==209 && avhash[24]==254 && avhash[25]==51 && avhash[26]==184 && avhash[27]==122 && avhash[28]==44 && avhash[29]==216 && avhash[30]==208 && avhash[31]==190)
            {
                return true;
            }
            else if(avhash[0]==210)
            {
                if(avhash[1]==141 && avhash[2]==143 && avhash[3]==143 && avhash[4]==121 && avhash[5]==242 && avhash[6]==214 && avhash[7]==117 && avhash[8]==33 && avhash[9]==151 && avhash[10]==70 && avhash[11]==216 && avhash[12]==136 && avhash[13]==224 && avhash[14]==217 && avhash[15]==76 && avhash[16]==218 && avhash[17]==207 && avhash[18]==124 && avhash[19]==218 && avhash[20]==91 && avhash[21]==185 && avhash[22]==43 && avhash[23]==21 && avhash[24]==205 && avhash[25]==153 && avhash[26]==82 && avhash[27]==228 && avhash[28]==185 && avhash[29]==6 && avhash[30]==128 && avhash[31]==205)
                {
                    return true;
                }
                else if(avhash[1]==218 && avhash[2]==25 && avhash[3]==239 && avhash[4]==142 && avhash[5]==79 && avhash[6]==121 && avhash[7]==123 && avhash[8]==73 && avhash[9]==170 && avhash[10]==143 && avhash[11]==78 && avhash[12]==155 && avhash[13]==208 && avhash[14]==20 && avhash[15]==248 && avhash[16]==137 && avhash[17]==165 && avhash[18]==53 && avhash[19]==82 && avhash[20]==134 && avhash[21]==241 && avhash[22]==153 && avhash[23]==58 && avhash[24]==119 && avhash[25]==126 && avhash[26]==220 && avhash[27]==21 && avhash[28]==187 && avhash[29]==250 && avhash[30]==140 && avhash[31]==32)
                {
                    return true;
                }
            }
            else if(avhash[0]==215 && avhash[1]==224 && avhash[2]==50 && avhash[3]==55 && avhash[4]==7 && avhash[5]==210 && avhash[6]==236 && avhash[7]==39 && avhash[8]==128 && avhash[9]==210 && avhash[10]==212 && avhash[11]==106 && avhash[12]==141 && avhash[13]==114 && avhash[14]==206 && avhash[15]==135 && avhash[16]==152 && avhash[17]==90 && avhash[18]==64 && avhash[19]==121 && avhash[20]==52 && avhash[21]==154 && avhash[22]==224 && avhash[23]==242 && avhash[24]==77 && avhash[25]==103 && avhash[26]==140 && avhash[27]==189 && avhash[28]==148 && avhash[29]==2 && avhash[30]==194 && avhash[31]==193)
            {
                return true;
            }
            else if(avhash[0]==218 && avhash[1]==97 && avhash[2]==148 && avhash[3]==82 && avhash[4]==229 && avhash[5]==69 && avhash[6]==30 && avhash[7]==149 && avhash[8]==180 && avhash[9]==123 && avhash[10]==45 && avhash[11]==126 && avhash[12]==51 && avhash[13]==111 && avhash[14]==41 && avhash[15]==226 && avhash[16]==233 && avhash[17]==244 && avhash[18]==206 && avhash[19]==65 && avhash[20]==57 && avhash[21]==195 && avhash[22]==177 && avhash[23]==120 && avhash[24]==184 && avhash[25]==56 && avhash[26]==249 && avhash[27]==139 && avhash[28]==28 && avhash[29]==70 && avhash[30]==159 && avhash[31]==13)
            {
                return true;
            }
            else if(avhash[0]==219 && avhash[1]==231 && avhash[2]==54 && avhash[3]==152 && avhash[4]==117 && avhash[5]==248 && avhash[6]==175 && avhash[7]==51 && avhash[8]==211 && avhash[9]==141 && avhash[10]==30 && avhash[11]==18 && avhash[12]==224 && avhash[13]==107 && avhash[14]==46 && avhash[15]==103 && avhash[16]==180 && avhash[17]==8 && avhash[18]==3 && avhash[19]==151 && avhash[20]==158 && avhash[21]==29 && avhash[22]==59 && avhash[23]==245 && avhash[24]==77 && avhash[25]==45 && avhash[26]==245 && avhash[27]==109 && avhash[28]==21 && avhash[29]==14 && avhash[30]==101 && avhash[31]==130)
            {
                return true;
            }
            else if(avhash[0]==220)
            {
                if(avhash[1]==133 && avhash[2]==111 && avhash[3]==16 && avhash[4]==89 && avhash[5]==90 && avhash[6]==177 && avhash[7]==196 && avhash[8]==242 && avhash[9]==181 && avhash[10]==112 && avhash[11]==210 && avhash[12]==215 && avhash[13]==100 && avhash[14]==59 && avhash[15]==109 && avhash[16]==9 && avhash[17]==215 && avhash[18]==10 && avhash[19]==250 && avhash[20]==39 && avhash[21]==200 && avhash[22]==188 && avhash[23]==59 && avhash[24]==7 && avhash[25]==20 && avhash[26]==134 && avhash[27]==148 && avhash[28]==48 && avhash[29]==113 && avhash[30]==166 && avhash[31]==31)
                {
                    return true;
                }
                else if(avhash[1]==72 && avhash[2]==240 && avhash[3]==1 && avhash[4]==189 && avhash[5]==129 && avhash[6]==248 && avhash[7]==145 && avhash[8]==108 && avhash[9]==29 && avhash[10]==145 && avhash[11]==154 && avhash[12]==146 && avhash[13]==194 && avhash[14]==238 && avhash[15]==189 && avhash[16]==16 && avhash[17]==21 && avhash[18]==61 && avhash[19]==63 && avhash[20]==228 && avhash[21]==22 && avhash[22]==132 && avhash[23]==5 && avhash[24]==226 && avhash[25]==4 && avhash[26]==16 && avhash[27]==41 && avhash[28]==99 && avhash[29]==61 && avhash[30]==211 && avhash[31]==222)
                {
                    return true;
                }
            }
            else if(avhash[0]==222)
            {
                if(avhash[1]==173 && avhash[2]==29 && avhash[3]==111 && avhash[4]==94 && avhash[5]==193 && avhash[6]==188 && avhash[7]==6 && avhash[8]==8 && avhash[9]==146 && avhash[10]==52 && avhash[11]==111 && avhash[12]==7 && avhash[13]==43 && avhash[14]==211 && avhash[15]==24 && avhash[16]==116 && avhash[17]==85 && avhash[18]==1 && avhash[19]==22 && avhash[20]==232 && avhash[21]==80 && avhash[22]==233 && avhash[23]==171 && avhash[24]==146 && avhash[25]==30 && avhash[26]==255 && avhash[27]==18 && avhash[28]==78 && avhash[29]==104 && avhash[30]==3 && avhash[31]==135)
                {
                    return true;
                }
                else if(avhash[1]==61 && avhash[2]==108 && avhash[3]==14 && avhash[4]==113 && avhash[5]==151 && avhash[6]==64 && avhash[7]==125 && avhash[8]==122 && avhash[9]==73 && avhash[10]==107 && avhash[11]==44 && avhash[12]==58 && avhash[13]==43 && avhash[14]==78 && avhash[15]==9 && avhash[16]==106 && avhash[17]==135 && avhash[18]==248 && avhash[19]==150 && avhash[20]==124 && avhash[21]==44 && avhash[22]==114 && avhash[23]==101 && avhash[24]==121 && avhash[25]==40 && avhash[26]==141 && avhash[27]==32 && avhash[28]==73 && avhash[29]==101 && avhash[30]==132 && avhash[31]==18)
                {
                    return true;
                }
            }
            else if(avhash[0]==225 && avhash[1]==63 && avhash[2]==231 && avhash[3]==56 && avhash[4]==152 && avhash[5]==169 && avhash[6]==56 && avhash[7]==241 && avhash[8]==151 && avhash[9]==134 && avhash[10]==42 && avhash[11]==207 && avhash[12]==167 && avhash[13]==61 && avhash[14]==97 && avhash[15]==13 && avhash[16]==7 && avhash[17]==154 && avhash[18]==8 && avhash[19]==46 && avhash[20]==139 && avhash[21]==202 && avhash[22]==73 && avhash[23]==124 && avhash[24]==254 && avhash[25]==64 && avhash[26]==124 && avhash[27]==32 && avhash[28]==41 && avhash[29]==54 && avhash[30]==197 && avhash[31]==65)
            {
                return true;
            }
            else if(avhash[0]==230 && avhash[1]==35 && avhash[2]==123 && avhash[3]==232 && avhash[4]==248 && avhash[5]==187 && avhash[6]==243 && avhash[7]==43 && avhash[8]==98 && avhash[9]==175 && avhash[10]==44 && avhash[11]==91 && avhash[12]==93 && avhash[13]==254 && avhash[14]==74 && avhash[15]==89 && avhash[16]==182 && avhash[17]==34 && avhash[18]==35 && avhash[19]==231 && avhash[20]==203 && avhash[21]==235 && avhash[22]==66 && avhash[23]==166 && avhash[24]==19 && avhash[25]==12 && avhash[26]==210 && avhash[27]==26 && avhash[28]==138 && avhash[29]==106 && avhash[30]==2 && avhash[31]==217)
            {
                return true;
            }
            else if(avhash[0]==243 && avhash[1]==14 && avhash[2]==180 && avhash[3]==169 && avhash[4]==120 && avhash[5]==215 && avhash[6]==182 && avhash[7]==168 && avhash[8]==92 && avhash[9]==244 && avhash[10]==230 && avhash[11]==84 && avhash[12]==82 && avhash[13]==204 && avhash[14]==100 && avhash[15]==198 && avhash[16]==96 && avhash[17]==230 && avhash[18]==29 && avhash[19]==36 && avhash[20]==1 && avhash[21]==122 && avhash[22]==213 && avhash[23]==10 && avhash[24]==158 && avhash[25]==184 && avhash[26]==137 && avhash[27]==29 && avhash[28]==118 && avhash[29]==203 && avhash[30]==172 && avhash[31]==30)
            {
                return true;
            }
            else if(avhash[0]==244 && avhash[1]==169 && avhash[2]==90 && avhash[3]==129 && avhash[4]==157 && avhash[5]==249 && avhash[6]==145 && avhash[7]==246 && avhash[8]==92 && avhash[9]==227 && avhash[10]==156 && avhash[11]==85 && avhash[12]==129 && avhash[13]==7 && avhash[14]==67 && avhash[15]==229 && avhash[16]==59 && avhash[17]==156 && avhash[18]==163 && avhash[19]==170 && avhash[20]==193 && avhash[21]==182 && avhash[22]==42 && avhash[23]==119 && avhash[24]==85 && avhash[25]==237 && avhash[26]==107 && avhash[27]==233 && avhash[28]==52 && avhash[29]==64 && avhash[30]==139 && avhash[31]==244)
            {
                return true;
            }
            else if(avhash[0]==245 && avhash[1]==91 && avhash[2]==187 && avhash[3]==126 && avhash[4]==37 && avhash[5]==115 && avhash[6]==47 && avhash[7]==116 && avhash[8]==102 && avhash[9]==21 && avhash[10]==66 && avhash[11]==202 && avhash[12]==42 && avhash[13]==7 && avhash[14]==159 && avhash[15]==100 && avhash[16]==48 && avhash[17]==120 && avhash[18]==55 && avhash[19]==36 && avhash[20]==246 && avhash[21]==240 && avhash[22]==230 && avhash[23]==101 && avhash[24]==91 && avhash[25]==183 && avhash[26]==72 && avhash[27]==151 && avhash[28]==65 && avhash[29]==142 && avhash[30]==95 && avhash[31]==102)
            {
                return true;
            }
            else if(avhash[0]==246)
            {
                if(avhash[1]==250 && avhash[2]==66 && avhash[3]==223 && avhash[4]==100 && avhash[5]==95 && avhash[6]==48 && avhash[7]==55 && avhash[8]==20 && avhash[9]==137 && avhash[10]==121 && avhash[11]==83 && avhash[12]==233 && avhash[13]==40 && avhash[14]==157 && avhash[15]==120 && avhash[16]==211 && avhash[17]==62 && avhash[18]==115 && avhash[19]==63 && avhash[20]==46 && avhash[21]==31 && avhash[22]==241 && avhash[23]==80 && avhash[24]==82 && avhash[25]==240 && avhash[26]==158 && avhash[27]==191 && avhash[28]==154 && avhash[29]==147 && avhash[30]==145 && avhash[31]==18)
                {
                    return true;
                }
                else if(avhash[1]==52 && avhash[2]==57 && avhash[3]==81 && avhash[4]==208 && avhash[5]==134 && avhash[6]==160 && avhash[7]==85 && avhash[8]==46 && avhash[9]==239 && avhash[10]==18 && avhash[11]==184 && avhash[12]==123 && avhash[13]==23 && avhash[14]==40 && avhash[15]==140 && avhash[16]==57 && avhash[17]==5 && avhash[18]==39 && avhash[19]==209 && avhash[20]==22 && avhash[21]==129 && avhash[22]==134 && avhash[23]==184 && avhash[24]==178 && avhash[25]==34 && avhash[26]==189 && avhash[27]==127 && avhash[28]==200 && avhash[29]==150 && avhash[30]==38 && avhash[31]==76)
                {
                    return true;
                }
            }
            else if(avhash[0]==247 && avhash[1]==238 && avhash[2]==132 && avhash[3]==20 && avhash[4]==91 && avhash[5]==157 && avhash[6]==86 && avhash[7]==155 && avhash[8]==208 && avhash[9]==181 && avhash[10]==77 && avhash[11]==10 && avhash[12]==58 && avhash[13]==36 && avhash[14]==28 && avhash[15]==54 && avhash[16]==151 && avhash[17]==255 && avhash[18]==243 && avhash[19]==193 && avhash[20]==74 && avhash[21]==199 && avhash[22]==114 && avhash[23]==144 && avhash[24]==224 && avhash[25]==27 && avhash[26]==220 && avhash[27]==56 && avhash[28]==247 && avhash[29]==39 && avhash[30]==11 && avhash[31]==188)
            {
                return true;
            }
            else if(avhash[0]==248 && avhash[1]==45 && avhash[2]==242 && avhash[3]==13 && avhash[4]==182 && avhash[5]==37 && avhash[6]==147 && avhash[7]==128 && avhash[8]==63 && avhash[9]==125 && avhash[10]==77 && avhash[11]==146 && avhash[12]==121 && avhash[13]==136 && avhash[14]==28 && avhash[15]==38 && avhash[16]==188 && avhash[17]==182 && avhash[18]==173 && avhash[19]==154 && avhash[20]==149 && avhash[21]==42 && avhash[22]==246 && avhash[23]==18 && avhash[24]==35 && avhash[25]==126 && avhash[26]==104 && avhash[27]==96 && avhash[28]==251 && avhash[29]==151 && avhash[30]==82 && avhash[31]==125)
            {
                return true;
            }
            else if(avhash[0]==25 && avhash[1]==218 && avhash[2]==106 && avhash[3]==173 && avhash[4]==188 && avhash[5]==40 && avhash[6]==248 && avhash[7]==32 && avhash[8]==48 && avhash[9]==151 && avhash[10]==121 && avhash[11]==25 && avhash[12]==126 && avhash[13]==75 && avhash[14]==241 && avhash[15]==167 && avhash[16]==48 && avhash[17]==57 && avhash[18]==122 && avhash[19]==219 && avhash[20]==118 && avhash[21]==51 && avhash[22]==78 && avhash[23]==62 && avhash[24]==122 && avhash[25]==47 && avhash[26]==191 && avhash[27]==127 && avhash[28]==20 && avhash[29]==209 && avhash[30]==225 && avhash[31]==138)
            {
                return true;
            }
            else if(avhash[0]==252 && avhash[1]==127 && avhash[2]==41 && avhash[3]==82 && avhash[4]==244 && avhash[5]==61 && avhash[6]==107 && avhash[7]==25 && avhash[8]==70 && avhash[9]==35 && avhash[10]==22 && avhash[11]==5 && avhash[12]==23 && avhash[13]==63 && avhash[14]==236 && avhash[15]==125 && avhash[16]==33 && avhash[17]==132 && avhash[18]==117 && avhash[19]==59 && avhash[20]==114 && avhash[21]==229 && avhash[22]==159 && avhash[23]==198 && avhash[24]==172 && avhash[25]==49 && avhash[26]==139 && avhash[27]==48 && avhash[28]==54 && avhash[29]==56 && avhash[30]==73 && avhash[31]==81)
            {
                return true;
            }
            else if(avhash[0]==29 && avhash[1]==104 && avhash[2]==115 && avhash[3]==26 && avhash[4]==61 && avhash[5]==182 && avhash[6]==171 && avhash[7]==109 && avhash[8]==112 && avhash[9]==209 && avhash[10]==228 && avhash[11]==214 && avhash[12]==152 && avhash[13]==161 && avhash[14]==187 && avhash[15]==199 && avhash[16]==242 && avhash[17]==238 && avhash[18]==152 && avhash[19]==40 && avhash[20]==77 && avhash[21]==213 && avhash[22]==21 && avhash[23]==247 && avhash[24]==245 && avhash[25]==102 && avhash[26]==148 && avhash[27]==94 && avhash[28]==101 && avhash[29]==37 && avhash[30]==113 && avhash[31]==46)
            {
                return true;
            }
            else if(avhash[0]==3)
            {
                if(avhash[1]==138 && avhash[2]==30 && avhash[3]==113 && avhash[4]==204 && avhash[5]==86 && avhash[6]==133 && avhash[7]==103 && avhash[8]==70 && avhash[9]==232 && avhash[10]==141 && avhash[11]==223 && avhash[12]==29 && avhash[13]==39 && avhash[14]==11 && avhash[15]==192 && avhash[16]==26 && avhash[17]==180 && avhash[18]==5 && avhash[19]==132 && avhash[20]==100 && avhash[21]==253 && avhash[22]==147 && avhash[23]==79 && avhash[24]==46 && avhash[25]==242 && avhash[26]==14 && avhash[27]==45 && avhash[28]==167 && avhash[29]==226 && avhash[30]==148 && avhash[31]==68)
                {
                    return true;
                }
                else if(avhash[1]==173 && avhash[2]==70 && avhash[3]==57 && avhash[4]==148 && avhash[5]==190 && avhash[6]==190 && avhash[7]==245 && avhash[8]==81 && avhash[9]==83 && avhash[10]==100 && avhash[11]==138 && avhash[12]==76 && avhash[13]==84 && avhash[14]==192 && avhash[15]==27 && avhash[16]==27 && avhash[17]==135 && avhash[18]==140 && avhash[19]==229 && avhash[20]==173 && avhash[21]==96 && avhash[22]==225 && avhash[23]==180 && avhash[24]==120 && avhash[25]==211 && avhash[26]==243 && avhash[27]==80 && avhash[28]==136 && avhash[29]==68 && avhash[30]==162 && avhash[31]==219)
                {
                    return true;
                }
            }
            else if(avhash[0]==31 && avhash[1]==253 && avhash[2]==99 && avhash[3]==218 && avhash[4]==86 && avhash[5]==109 && avhash[6]==125 && avhash[7]==67 && avhash[8]==172 && avhash[9]==202 && avhash[10]==68 && avhash[11]==119 && avhash[12]==220 && avhash[13]==75 && avhash[14]==152 && avhash[15]==64 && avhash[16]==205 && avhash[17]==205 && avhash[18]==16 && avhash[19]==35 && avhash[20]==188 && avhash[21]==243 && avhash[22]==72 && avhash[23]==19 && avhash[24]==188 && avhash[25]==205 && avhash[26]==96 && avhash[27]==135 && avhash[28]==90 && avhash[29]==135 && avhash[30]==149 && avhash[31]==111)
            {
                return true;
            }
            else if(avhash[0]==32 && avhash[1]==247 && avhash[2]==86 && avhash[3]==138 && avhash[4]==178 && avhash[5]==242 && avhash[6]==126 && avhash[7]==161 && avhash[8]==142 && avhash[9]==78 && avhash[10]==13 && avhash[11]==170 && avhash[12]==27 && avhash[13]==171 && avhash[14]==38 && avhash[15]==153 && avhash[16]==234 && avhash[17]==130 && avhash[18]==133 && avhash[19]==96 && avhash[20]==101 && avhash[21]==121 && avhash[22]==11 && avhash[23]==241 && avhash[24]==153 && avhash[25]==226 && avhash[26]==112 && avhash[27]==2 && avhash[28]==15 && avhash[29]==74 && avhash[30]==200 && avhash[31]==80)
            {
                return true;
            }
            else if(avhash[0]==37 && avhash[1]==236 && avhash[2]==253 && avhash[3]==190 && avhash[4]==112 && avhash[5]==164 && avhash[6]==174 && avhash[7]==212 && avhash[8]==255 && avhash[9]==217 && avhash[10]==29 && avhash[11]==26 && avhash[12]==69 && avhash[13]==13 && avhash[14]==91 && avhash[15]==145 && avhash[16]==69 && avhash[17]==82 && avhash[18]==80 && avhash[19]==82 && avhash[20]==217 && avhash[21]==211 && avhash[22]==35 && avhash[23]==106 && avhash[24]==16 && avhash[25]==172 && avhash[26]==117 && avhash[27]==211 && avhash[28]==92 && avhash[29]==157 && avhash[30]==222 && avhash[31]==151)
            {
                return true;
            }
            else if(avhash[0]==38 && avhash[1]==124 && avhash[2]==175 && avhash[3]==213 && avhash[4]==19 && avhash[5]==76 && avhash[6]==60 && avhash[7]==10 && avhash[8]==165 && avhash[9]==10 && avhash[10]==3 && avhash[11]==12 && avhash[12]==165 && avhash[13]==110 && avhash[14]==113 && avhash[15]==4 && avhash[16]==18 && avhash[17]==199 && avhash[18]==10 && avhash[19]==215 && avhash[20]==32 && avhash[21]==104 && avhash[22]==116 && avhash[23]==142 && avhash[24]==240 && avhash[25]==143 && avhash[26]==144 && avhash[27]==163 && avhash[28]==46 && avhash[29]==166 && avhash[30]==32 && avhash[31]==197)
            {
                return true;
            }
            else if(avhash[0]==4 && avhash[1]==211 && avhash[2]==188 && avhash[3]==42 && avhash[4]==123 && avhash[5]==229 && avhash[6]==6 && avhash[7]==242 && avhash[8]==9 && avhash[9]==62 && avhash[10]==139 && avhash[11]==86 && avhash[12]==150 && avhash[13]==194 && avhash[14]==125 && avhash[15]==23 && avhash[16]==167 && avhash[17]==12 && avhash[18]==128 && avhash[19]==233 && avhash[20]==6 && avhash[21]==36 && avhash[22]==172 && avhash[23]==88 && avhash[24]==165 && avhash[25]==29 && avhash[26]==89 && avhash[27]==90 && avhash[28]==169 && avhash[29]==205 && avhash[30]==0 && avhash[31]==139)
            {
                return true;
            }
            else if(avhash[0]==43 && avhash[1]==93 && avhash[2]==39 && avhash[3]==140 && avhash[4]==165 && avhash[5]==26 && avhash[6]==94 && avhash[7]==75 && avhash[8]==253 && avhash[9]==133 && avhash[10]==68 && avhash[11]==195 && avhash[12]==156 && avhash[13]==134 && avhash[14]==114 && avhash[15]==250 && avhash[16]==148 && avhash[17]==59 && avhash[18]==104 && avhash[19]==114 && avhash[20]==226 && avhash[21]==201 && avhash[22]==191 && avhash[23]==176 && avhash[24]==148 && avhash[25]==128 && avhash[26]==84 && avhash[27]==65 && avhash[28]==237 && avhash[29]==240 && avhash[30]==47 && avhash[31]==56)
            {
                return true;
            }
            else if(avhash[0]==44)
            {
                if(avhash[1]==102 && avhash[2]==163 && avhash[3]==160 && avhash[4]==1 && avhash[5]==155 && avhash[6]==225 && avhash[7]==96 && avhash[8]==116 && avhash[9]==169 && avhash[10]==24 && avhash[11]==230 && avhash[12]==242 && avhash[13]==11 && avhash[14]==43 && avhash[15]==221 && avhash[16]==116 && avhash[17]==124 && avhash[18]==73 && avhash[19]==29 && avhash[20]==72 && avhash[21]==85 && avhash[22]==247 && avhash[23]==211 && avhash[24]==228 && avhash[25]==161 && avhash[26]==70 && avhash[27]==243 && avhash[28]==246 && avhash[29]==205 && avhash[30]==238 && avhash[31]==139)
                {
                    return true;
                }
                else if(avhash[1]==87 && avhash[2]==138 && avhash[3]==37 && avhash[4]==223 && avhash[5]==167 && avhash[6]==176 && avhash[7]==207 && avhash[8]==43 && avhash[9]==165 && avhash[10]==185 && avhash[11]==99 && avhash[12]==80 && avhash[13]==144 && avhash[14]==214 && avhash[15]==66 && avhash[16]==101 && avhash[17]==249 && avhash[18]==51 && avhash[19]==64 && avhash[20]==132 && avhash[21]==132 && avhash[22]==92 && avhash[23]==146 && avhash[24]==130 && avhash[25]==10 && avhash[26]==94 && avhash[27]==193 && avhash[28]==11 && avhash[29]==157 && avhash[30]==53 && avhash[31]==4)
                {
                    return true;
                }
            }
            else if(avhash[0]==47 && avhash[1]==131 && avhash[2]==149 && avhash[3]==175 && avhash[4]==209 && avhash[5]==228 && avhash[6]==154 && avhash[7]==30 && avhash[8]==115 && avhash[9]==177 && avhash[10]==117 && avhash[11]==197 && avhash[12]==99 && avhash[13]==150 && avhash[14]==22 && avhash[15]==39 && avhash[16]==0 && avhash[17]==162 && avhash[18]==174 && avhash[19]==246 && avhash[20]==82 && avhash[21]==86 && avhash[22]==116 && avhash[23]==118 && avhash[24]==76 && avhash[25]==195 && avhash[26]==210 && avhash[27]==124 && avhash[28]==255 && avhash[29]==107 && avhash[30]==23 && avhash[31]==99)
            {
                return true;
            }
            else if(avhash[0]==48)
            {
                if(avhash[1]==190 && avhash[2]==7 && avhash[3]==186 && avhash[4]==116 && avhash[5]==133 && avhash[6]==240 && avhash[7]==85 && avhash[8]==44 && avhash[9]==11 && avhash[10]==218 && avhash[11]==190 && avhash[12]==204 && avhash[13]==84 && avhash[14]==160 && avhash[15]==85 && avhash[16]==98 && avhash[17]==237 && avhash[18]==203 && avhash[19]==147 && avhash[20]==245 && avhash[21]==128 && avhash[22]==81 && avhash[23]==202 && avhash[24]==1 && avhash[25]==143 && avhash[26]==0 && avhash[27]==37 && avhash[28]==110 && avhash[29]==82 && avhash[30]==231 && avhash[31]==205)
                {
                    return true;
                }
                else if(avhash[1]==91 && avhash[2]==63 && avhash[3]==231 && avhash[4]==172 && avhash[5]==165 && avhash[6]==210 && avhash[7]==98 && avhash[8]==50 && avhash[9]==205 && avhash[10]==60 && avhash[11]==63 && avhash[12]==110 && avhash[13]==167 && avhash[14]==102 && avhash[15]==89 && avhash[16]==31 && avhash[17]==214 && avhash[18]==87 && avhash[19]==42 && avhash[20]==56 && avhash[21]==27 && avhash[22]==164 && avhash[23]==110 && avhash[24]==211 && avhash[25]==236 && avhash[26]==137 && avhash[27]==229 && avhash[28]==169 && avhash[29]==237 && avhash[30]==209 && avhash[31]==83)
                {
                    return true;
                }
            }
            else if(avhash[0]==5 && avhash[1]==246 && avhash[2]==106 && avhash[3]==179 && avhash[4]==236 && avhash[5]==222 && avhash[6]==121 && avhash[7]==38 && avhash[8]==9 && avhash[9]==126 && avhash[10]==156 && avhash[11]==154 && avhash[12]==229 && avhash[13]==25 && avhash[14]==10 && avhash[15]==24 && avhash[16]==208 && avhash[17]==148 && avhash[18]==57 && avhash[19]==97 && avhash[20]==160 && avhash[21]==117 && avhash[22]==115 && avhash[23]==81 && avhash[24]==230 && avhash[25]==44 && avhash[26]==191 && avhash[27]==109 && avhash[28]==139 && avhash[29]==217 && avhash[30]==51 && avhash[31]==201)
            {
                return true;
            }
            else if(avhash[0]==57)
            {
                if(avhash[1]==147 && avhash[2]==233 && avhash[3]==30 && avhash[4]==83 && avhash[5]==136 && avhash[6]==239 && avhash[7]==55 && avhash[8]==121 && avhash[9]==32 && avhash[10]==143 && avhash[11]==232 && avhash[12]==160 && avhash[13]==29 && avhash[14]==155 && avhash[15]==156 && avhash[16]==94 && avhash[17]==215 && avhash[18]==83 && avhash[19]==29 && avhash[20]==162 && avhash[21]==40 && avhash[22]==110 && avhash[23]==41 && avhash[24]==255 && avhash[25]==27 && avhash[26]==154 && avhash[27]==251 && avhash[28]==82 && avhash[29]==30 && avhash[30]==68 && avhash[31]==39)
                {
                    return true;
                }
                else if(avhash[1]==169 && avhash[2]==6 && avhash[3]==177 && avhash[4]==119 && avhash[5]==190 && avhash[6]==33 && avhash[7]==240 && avhash[8]==212 && avhash[9]==242 && avhash[10]==178 && avhash[11]==116 && avhash[12]==191 && avhash[13]==147 && avhash[14]==201 && avhash[15]==221 && avhash[16]==196 && avhash[17]==32 && avhash[18]==109 && avhash[19]==184 && avhash[20]==19 && avhash[21]==176 && avhash[22]==169 && avhash[23]==100 && avhash[24]==61 && avhash[25]==70 && avhash[26]==154 && avhash[27]==47 && avhash[28]==10 && avhash[29]==107 && avhash[30]==55 && avhash[31]==242)
                {
                    return true;
                }
            }
            else if(avhash[0]==58 && avhash[1]==36 && avhash[2]==152 && avhash[3]==68 && avhash[4]==51 && avhash[5]==89 && avhash[6]==1 && avhash[7]==3 && avhash[8]==206 && avhash[9]==52 && avhash[10]==8 && avhash[11]==90 && avhash[12]==21 && avhash[13]==123 && avhash[14]==244 && avhash[15]==33 && avhash[16]==125 && avhash[17]==23 && avhash[18]==113 && avhash[19]==11 && avhash[20]==239 && avhash[21]==49 && avhash[22]==3 && avhash[23]==14 && avhash[24]==107 && avhash[25]==58 && avhash[26]==208 && avhash[27]==182 && avhash[28]==87 && avhash[29]==86 && avhash[30]==166 && avhash[31]==131)
            {
                return true;
            }
            else if(avhash[0]==64 && avhash[1]==33 && avhash[2]==190 && avhash[3]==239 && avhash[4]==166 && avhash[5]==161 && avhash[6]==34 && avhash[7]==142 && avhash[8]==115 && avhash[9]==93 && avhash[10]==176 && avhash[11]==187 && avhash[12]==34 && avhash[13]==19 && avhash[14]==228 && avhash[15]==181 && avhash[16]==214 && avhash[17]==140 && avhash[18]==97 && avhash[19]==242 && avhash[20]==170 && avhash[21]==170 && avhash[22]==175 && avhash[23]==30 && avhash[24]==187 && avhash[25]==67 && avhash[26]==104 && avhash[27]==62 && avhash[28]==201 && avhash[29]==193 && avhash[30]==212 && avhash[31]==96)
            {
                return true;
            }
            else if(avhash[0]==66 && avhash[1]==122 && avhash[2]==10 && avhash[3]==199 && avhash[4]==113 && avhash[5]==54 && avhash[6]==146 && avhash[7]==81 && avhash[8]==38 && avhash[9]==236 && avhash[10]==180 && avhash[11]==219 && avhash[12]==50 && avhash[13]==101 && avhash[14]==183 && avhash[15]==31 && avhash[16]==93 && avhash[17]==201 && avhash[18]==250 && avhash[19]==139 && avhash[20]==201 && avhash[21]==246 && avhash[22]==141 && avhash[23]==190 && avhash[24]==236 && avhash[25]==187 && avhash[26]==254 && avhash[27]==197 && avhash[28]==168 && avhash[29]==66 && avhash[30]==254 && avhash[31]==133)
            {
                return true;
            }
            else if(avhash[0]==67 && avhash[1]==216 && avhash[2]==185 && avhash[3]==83 && avhash[4]==61 && avhash[5]==93 && avhash[6]==74 && avhash[7]==93 && avhash[8]==112 && avhash[9]==248 && avhash[10]==61 && avhash[11]==196 && avhash[12]==170 && avhash[13]==161 && avhash[14]==39 && avhash[15]==207 && avhash[16]==74 && avhash[17]==66 && avhash[18]==5 && avhash[19]==239 && avhash[20]==159 && avhash[21]==74 && avhash[22]==12 && avhash[23]==172 && avhash[24]==36 && avhash[25]==243 && avhash[26]==238 && avhash[27]==4 && avhash[28]==14 && avhash[29]==55 && avhash[30]==252 && avhash[31]==149)
            {
                return true;
            }
            else if(avhash[0]==68 && avhash[1]==49 && avhash[2]==65 && avhash[3]==16 && avhash[4]==46 && avhash[5]==189 && avhash[6]==240 && avhash[7]==239 && avhash[8]==93 && avhash[9]==100 && avhash[10]==87 && avhash[11]==75 && avhash[12]==241 && avhash[13]==47 && avhash[14]==108 && avhash[15]==102 && avhash[16]==110 && avhash[17]==129 && avhash[18]==242 && avhash[19]==21 && avhash[20]==158 && avhash[21]==201 && avhash[22]==87 && avhash[23]==155 && avhash[24]==247 && avhash[25]==253 && avhash[26]==148 && avhash[27]==208 && avhash[28]==245 && avhash[29]==96 && avhash[30]==253 && avhash[31]==66)
            {
                return true;
            }
            else if(avhash[0]==69 && avhash[1]==13 && avhash[2]==223 && avhash[3]==22 && avhash[4]==4 && avhash[5]==217 && avhash[6]==210 && avhash[7]==145 && avhash[8]==59 && avhash[9]==59 && avhash[10]==174 && avhash[11]==193 && avhash[12]==210 && avhash[13]==185 && avhash[14]==195 && avhash[15]==137 && avhash[16]==172 && avhash[17]==81 && avhash[18]==209 && avhash[19]==131 && avhash[20]==86 && avhash[21]==104 && avhash[22]==116 && avhash[23]==202 && avhash[24]==142 && avhash[25]==241 && avhash[26]==223 && avhash[27]==213 && avhash[28]==183 && avhash[29]==159 && avhash[30]==241 && avhash[31]==80)
            {
                return true;
            }
            else if(avhash[0]==71)
            {
                if(avhash[1]==19 && avhash[2]==58 && avhash[3]==221 && avhash[4]==130 && avhash[5]==89 && avhash[6]==227 && avhash[7]==204 && avhash[8]==186 && avhash[9]==67 && avhash[10]==239 && avhash[11]==215 && avhash[12]==67 && avhash[13]==104 && avhash[14]==135 && avhash[15]==108 && avhash[16]==112 && avhash[17]==126 && avhash[18]==50 && avhash[19]==19 && avhash[20]==235 && avhash[21]==145 && avhash[22]==205 && avhash[23]==53 && avhash[24]==192 && avhash[25]==48 && avhash[26]==90 && avhash[27]==96 && avhash[28]==12 && avhash[29]==44 && avhash[30]==167 && avhash[31]==15)
                {
                    return true;
                }
                else if(avhash[1]==54 && avhash[2]==254 && avhash[3]==12 && avhash[4]==70 && avhash[5]==165 && avhash[6]==233 && avhash[7]==87 && avhash[8]==185 && avhash[9]==50 && avhash[10]==39 && avhash[11]==198 && avhash[12]==125 && avhash[13]==135 && avhash[14]==29 && avhash[15]==155 && avhash[16]==169 && avhash[17]==53 && avhash[18]==209 && avhash[19]==125 && avhash[20]==4 && avhash[21]==92 && avhash[22]==81 && avhash[23]==20 && avhash[24]==30 && avhash[25]==87 && avhash[26]==7 && avhash[27]==71 && avhash[28]==117 && avhash[29]==105 && avhash[30]==200 && avhash[31]==215)
                {
                    return true;
                }
            }
            else if(avhash[0]==75 && avhash[1]==116 && avhash[2]==199 && avhash[3]==54 && avhash[4]==31 && avhash[5]==118 && avhash[6]==33 && avhash[7]==235 && avhash[8]==69 && avhash[9]==118 && avhash[10]==148 && avhash[11]==180 && avhash[12]==181 && avhash[13]==196 && avhash[14]==226 && avhash[15]==143 && avhash[16]==121 && avhash[17]==246 && avhash[18]==183 && avhash[19]==200 && avhash[20]==213 && avhash[21]==227 && avhash[22]==16 && avhash[23]==107 && avhash[24]==95 && avhash[25]==176 && avhash[26]==0 && avhash[27]==203 && avhash[28]==157 && avhash[29]==93 && avhash[30]==86 && avhash[31]==148)
            {
                return true;
            }
            else if(avhash[0]==77 && avhash[1]==95 && avhash[2]==119 && avhash[3]==173 && avhash[4]==5 && avhash[5]==62 && avhash[6]==15 && avhash[7]==187 && avhash[8]==98 && avhash[9]==93 && avhash[10]==174 && avhash[11]==41 && avhash[12]==102 && avhash[13]==96 && avhash[14]==172 && avhash[15]==142 && avhash[16]==166 && avhash[17]==117 && avhash[18]==81 && avhash[19]==165 && avhash[20]==231 && avhash[21]==93 && avhash[22]==160 && avhash[23]==197 && avhash[24]==12 && avhash[25]==105 && avhash[26]==14 && avhash[27]==130 && avhash[28]==79 && avhash[29]==39 && avhash[30]==99 && avhash[31]==194)
            {
                return true;
            }
            else if(avhash[0]==78 && avhash[1]==175 && avhash[2]==11 && avhash[3]==168 && avhash[4]==32 && avhash[5]==255 && avhash[6]==229 && avhash[7]==89 && avhash[8]==31 && avhash[9]==187 && avhash[10]==210 && avhash[11]==233 && avhash[12]==123 && avhash[13]==44 && avhash[14]==96 && avhash[15]==207 && avhash[16]==41 && avhash[17]==154 && avhash[18]==73 && avhash[19]==64 && avhash[20]==27 && avhash[21]==37 && avhash[22]==95 && avhash[23]==177 && avhash[24]==73 && avhash[25]==96 && avhash[26]==76 && avhash[27]==18 && avhash[28]==3 && avhash[29]==56 && avhash[30]==69 && avhash[31]==136)
            {
                return true;
            }
            else if(avhash[0]==79 && avhash[1]==191 && avhash[2]==12 && avhash[3]==54 && avhash[4]==161 && avhash[5]==176 && avhash[6]==62 && avhash[7]==114 && avhash[8]==92 && avhash[9]==111 && avhash[10]==63 && avhash[11]==45 && avhash[12]==83 && avhash[13]==30 && avhash[14]==11 && avhash[15]==38 && avhash[16]==193 && avhash[17]==150 && avhash[18]==1 && avhash[19]==74 && avhash[20]==176 && avhash[21]==142 && avhash[22]==193 && avhash[23]==26 && avhash[24]==40 && avhash[25]==229 && avhash[26]==55 && avhash[27]==60 && avhash[28]==122 && avhash[29]==122 && avhash[30]==225 && avhash[31]==139)
            {
                return true;
            }
            else if(avhash[0]==84)
            {
                if(avhash[1]==193 && avhash[2]==247 && avhash[3]==227 && avhash[4]==144 && avhash[5]==83 && avhash[6]==22 && avhash[7]==111 && avhash[8]==44 && avhash[9]==215 && avhash[10]==152 && avhash[11]==98 && avhash[12]==162 && avhash[13]==190 && avhash[14]==156 && avhash[15]==155 && avhash[16]==236 && avhash[17]==42 && avhash[18]==171 && avhash[19]==149 && avhash[20]==141 && avhash[21]==150 && avhash[22]==53 && avhash[23]==213 && avhash[24]==231 && avhash[25]==175 && avhash[26]==210 && avhash[27]==26 && avhash[28]==189 && avhash[29]==10 && avhash[30]==194 && avhash[31]==96)
                {
                    return true;
                }
                else if(avhash[1]==78 && avhash[2]==57 && avhash[3]==129 && avhash[4]==142 && avhash[5]==131 && avhash[6]==11 && avhash[7]==20 && avhash[8]==239 && avhash[9]==121 && avhash[10]==91 && avhash[11]==237 && avhash[12]==29 && avhash[13]==37 && avhash[14]==177 && avhash[15]==2 && avhash[16]==231 && avhash[17]==20 && avhash[18]==119 && avhash[19]==157 && avhash[20]==181 && avhash[21]==92 && avhash[22]==198 && avhash[23]==229 && avhash[24]==93 && avhash[25]==26 && avhash[26]==213 && avhash[27]==147 && avhash[28]==117 && avhash[29]==25 && avhash[30]==127 && avhash[31]==116)
                {
                    return true;
                }
            }
            else if(avhash[0]==86 && avhash[1]==213 && avhash[2]==54 && avhash[3]==107 && avhash[4]==224 && avhash[5]==246 && avhash[6]==220 && avhash[7]==237 && avhash[8]==60 && avhash[9]==138 && avhash[10]==27 && avhash[11]==56 && avhash[12]==162 && avhash[13]==208 && avhash[14]==44 && avhash[15]==189 && avhash[16]==32 && avhash[17]==217 && avhash[18]==232 && avhash[19]==196 && avhash[20]==79 && avhash[21]==247 && avhash[22]==77 && avhash[23]==85 && avhash[24]==243 && avhash[25]==158 && avhash[26]==31 && avhash[27]==16 && avhash[28]==179 && avhash[29]==15 && avhash[30]==214 && avhash[31]==40)
            {
                return true;
            }
            else if(avhash[0]==89 && avhash[1]==35 && avhash[2]==220 && avhash[3]==177 && avhash[4]==240 && avhash[5]==130 && avhash[6]==71 && avhash[7]==123 && avhash[8]==87 && avhash[9]==73 && avhash[10]==194 && avhash[11]==162 && avhash[12]==163 && avhash[13]==179 && avhash[14]==77 && avhash[15]==161 && avhash[16]==156 && avhash[17]==49 && avhash[18]==151 && avhash[19]==225 && avhash[20]==72 && avhash[21]==179 && avhash[22]==121 && avhash[23]==30 && avhash[24]==125 && avhash[25]==166 && avhash[26]==85 && avhash[27]==40 && avhash[28]==117 && avhash[29]==138 && avhash[30]==207 && avhash[31]==140)
            {
                return true;
            }
            else if(avhash[0]==90 && avhash[1]==147 && avhash[2]==200 && avhash[3]==200 && avhash[4]==150 && avhash[5]==100 && avhash[6]==177 && avhash[7]==158 && avhash[8]==223 && avhash[9]==105 && avhash[10]==205 && avhash[11]==18 && avhash[12]==244 && avhash[13]==66 && avhash[14]==107 && avhash[15]==39 && avhash[16]==149 && avhash[17]==14 && avhash[18]==203 && avhash[19]==236 && avhash[20]==174 && avhash[21]==221 && avhash[22]==214 && avhash[23]==24 && avhash[24]==15 && avhash[25]==5 && avhash[26]==112 && avhash[27]==209 && avhash[28]==135 && avhash[29]==120 && avhash[30]==82 && avhash[31]==156)
            {
                return true;
            }
            else if(avhash[0]==92 && avhash[1]==2 && avhash[2]==223 && avhash[3]==214 && avhash[4]==250 && avhash[5]==203 && avhash[6]==210 && avhash[7]==86 && avhash[8]==177 && avhash[9]==19 && avhash[10]==109 && avhash[11]==127 && avhash[12]==31 && avhash[13]==228 && avhash[14]==137 && avhash[15]==221 && avhash[16]==159 && avhash[17]==132 && avhash[18]==61 && avhash[19]==211 && avhash[20]==57 && avhash[21]==246 && avhash[22]==79 && avhash[23]==112 && avhash[24]==195 && avhash[25]==242 && avhash[26]==240 && avhash[27]==163 && avhash[28]==122 && avhash[29]==182 && avhash[30]==183 && avhash[31]==6)
            {
                return true;
            }
            else if(avhash[0]==93 && avhash[1]==36 && avhash[2]==66 && avhash[3]==231 && avhash[4]==116 && avhash[5]==146 && avhash[6]==183 && avhash[7]==201 && avhash[8]==39 && avhash[9]==250 && avhash[10]==143 && avhash[11]==151 && avhash[12]==93 && avhash[13]==37 && avhash[14]==87 && avhash[15]==228 && avhash[16]==66 && avhash[17]==7 && avhash[18]==28 && avhash[19]==126 && avhash[20]==141 && avhash[21]==87 && avhash[22]==9 && avhash[23]==231 && avhash[24]==171 && avhash[25]==224 && avhash[26]==238 && avhash[27]==31 && avhash[28]==16 && avhash[29]==99 && avhash[30]==45 && avhash[31]==17)
            {
                return true;
            }
            else if(avhash[0]==95 && avhash[1]==60 && avhash[2]==55 && avhash[3]==74 && avhash[4]==20 && avhash[5]==83 && avhash[6]==36 && avhash[7]==143 && avhash[8]==80 && avhash[9]==132 && avhash[10]==113 && avhash[11]==184 && avhash[12]==248 && avhash[13]==174 && avhash[14]==80 && avhash[15]==63 && avhash[16]==164 && avhash[17]==181 && avhash[18]==51 && avhash[19]==39 && avhash[20]==155 && avhash[21]==97 && avhash[22]==103 && avhash[23]==150 && avhash[24]==8 && avhash[25]==129 && avhash[26]==3 && avhash[27]==152 && avhash[28]==222 && avhash[29]==239 && avhash[30]==15 && avhash[31]==236)
            {
                return true;
            }
            else if(avhash[0]==96 && avhash[1]==132 && avhash[2]==179 && avhash[3]==51 && avhash[4]==144 && avhash[5]==242 && avhash[6]==89 && avhash[7]==31 && avhash[8]==59 && avhash[9]==137 && avhash[10]==168 && avhash[11]==76 && avhash[12]==41 && avhash[13]==74 && avhash[14]==156 && avhash[15]==213 && avhash[16]==177 && avhash[17]==67 && avhash[18]==51 && avhash[19]==129 && avhash[20]==66 && avhash[21]==170 && avhash[22]==13 && avhash[23]==44 && avhash[24]==221 && avhash[25]==201 && avhash[26]==164 && avhash[27]==209 && avhash[28]==122 && avhash[29]==22 && avhash[30]==31 && avhash[31]==161)
            {
                return true;
            }
            else if(avhash[0]==99)
            {
                if(avhash[1]==142 && avhash[2]==92 && avhash[3]==78 && avhash[4]==2 && avhash[5]==255 && avhash[6]==15 && avhash[7]==101 && avhash[8]==12 && avhash[9]==39 && avhash[10]==67 && avhash[11]==122 && avhash[12]==234 && avhash[13]==130 && avhash[14]==85 && avhash[15]==0 && avhash[16]==239 && avhash[17]==92 && avhash[18]==190 && avhash[19]==169 && avhash[20]==163 && avhash[21]==251 && avhash[22]==108 && avhash[23]==82 && avhash[24]==58 && avhash[25]==94 && avhash[26]==85 && avhash[27]==65 && avhash[28]==142 && avhash[29]==198 && avhash[30]==134 && avhash[31]==174)
                {
                    return true;
                }
                else if(avhash[1]==158 && avhash[2]==177 && avhash[3]==65 && avhash[4]==70 && avhash[5]==39 && avhash[6]==121 && avhash[7]==31 && avhash[8]==225 && avhash[9]==205 && avhash[10]==149 && avhash[11]==181 && avhash[12]==161 && avhash[13]==34 && avhash[14]==164 && avhash[15]==219 && avhash[16]==42 && avhash[17]==55 && avhash[18]==69 && avhash[19]==194 && avhash[20]==42 && avhash[21]==141 && avhash[22]==163 && avhash[23]==176 && avhash[24]==255 && avhash[25]==57 && avhash[26]==219 && avhash[27]==31 && avhash[28]==184 && avhash[29]==123 && avhash[30]==112 && avhash[31]==54)
                {
                    return true;
                }
            }


            // Check against user shitlist
            if (UserShitList.Contains(avID))
                return true;

            return false;
        }



        internal static bool IsBundleACrasher(byte[] hash)
        {
            if(hash[0]==101)
            {
                if(hash[1]==14 && hash[2]==124 && hash[3]==208 && hash[4]==203 && hash[5]==108 && hash[6]==174 && hash[7]==201 && hash[8]==93 && hash[9]==141 && hash[10]==8 && hash[11]==157 && hash[12]==221 && hash[13]==149 && hash[14]==100 && hash[15]==55 && hash[16]==164 && hash[17]==77 && hash[18]==30 && hash[19]==239 && hash[20]==102 && hash[21]==2 && hash[22]==16 && hash[23]==127 && hash[24]==251 && hash[25]==98 && hash[26]==92 && hash[27]==252 && hash[28]==170 && hash[29]==107 && hash[30]==47 && hash[31]==217)
                {
                    return true;
                }
                else if(hash[1]==16 && hash[2]==7 && hash[3]==110 && hash[4]==80 && hash[5]==61 && hash[6]==129 && hash[7]==35 && hash[8]==42 && hash[9]==235 && hash[10]==215 && hash[11]==242 && hash[12]==117 && hash[13]==46 && hash[14]==111 && hash[15]==23 && hash[16]==151 && hash[17]==24 && hash[18]==189 && hash[19]==194 && hash[20]==255 && hash[21]==64 && hash[22]==129 && hash[23]==5 && hash[24]==134 && hash[25]==234 && hash[26]==94 && hash[27]==97 && hash[28]==246 && hash[29]==59 && hash[30]==79 && hash[31]==210)
                {
                    return true;
                }
            }
            else if(hash[0]==102)
            {
                if(hash[1]==63 && hash[2]==182 && hash[3]==114 && hash[4]==193 && hash[5]==52 && hash[6]==183 && hash[7]==30 && hash[8]==96 && hash[9]==132 && hash[10]==88 && hash[11]==124 && hash[12]==244 && hash[13]==95 && hash[14]==95 && hash[15]==88 && hash[16]==33 && hash[17]==160 && hash[18]==67 && hash[19]==239 && hash[20]==211 && hash[21]==138 && hash[22]==151 && hash[23]==161 && hash[24]==169 && hash[25]==10 && hash[26]==98 && hash[27]==142 && hash[28]==229 && hash[29]==36 && hash[30]==154 && hash[31]==182)
                {
                    return true;
                }
                else if(hash[1]==97 && hash[2]==135 && hash[3]==191 && hash[4]==199 && hash[5]==28 && hash[6]==51 && hash[7]==142 && hash[8]==20 && hash[9]==222 && hash[10]==135 && hash[11]==226 && hash[12]==136 && hash[13]==113 && hash[14]==149 && hash[15]==5 && hash[16]==84 && hash[17]==235 && hash[18]==58 && hash[19]==93 && hash[20]==244 && hash[21]==108 && hash[22]==112 && hash[23]==16 && hash[24]==4 && hash[25]==76 && hash[26]==203 && hash[27]==181 && hash[28]==75 && hash[29]==3 && hash[30]==11 && hash[31]==119)
                {
                    return true;
                }
            }
            else if(hash[0]==112 && hash[1]==107 && hash[2]==166 && hash[3]==173 && hash[4]==127 && hash[5]==207 && hash[6]==121 && hash[7]==69 && hash[8]==113 && hash[9]==212 && hash[10]==102 && hash[11]==164 && hash[12]==107 && hash[13]==78 && hash[14]==245 && hash[15]==206 && hash[16]==18 && hash[17]==73 && hash[18]==248 && hash[19]==112 && hash[20]==65 && hash[21]==221 && hash[22]==125 && hash[23]==108 && hash[24]==104 && hash[25]==212 && hash[26]==254 && hash[27]==237 && hash[28]==246 && hash[29]==200 && hash[30]==9 && hash[31]==108)
            {
                return true;
            }
            else if(hash[0]==118 && hash[1]==59 && hash[2]==239 && hash[3]==222 && hash[4]==211 && hash[5]==247 && hash[6]==197 && hash[7]==197 && hash[8]==151 && hash[9]==233 && hash[10]==117 && hash[11]==25 && hash[12]==99 && hash[13]==143 && hash[14]==195 && hash[15]==40 && hash[16]==0 && hash[17]==58 && hash[18]==185 && hash[19]==157 && hash[20]==78 && hash[21]==154 && hash[22]==103 && hash[23]==108 && hash[24]==164 && hash[25]==210 && hash[26]==137 && hash[27]==70 && hash[28]==140 && hash[29]==153 && hash[30]==133 && hash[31]==34)
            {
                return true;
            }
            else if(hash[0]==121 && hash[1]==23 && hash[2]==157 && hash[3]==188 && hash[4]==75 && hash[5]==255 && hash[6]==98 && hash[7]==160 && hash[8]==24 && hash[9]==4 && hash[10]==95 && hash[11]==66 && hash[12]==107 && hash[13]==22 && hash[14]==251 && hash[15]==203 && hash[16]==128 && hash[17]==110 && hash[18]==166 && hash[19]==208 && hash[20]==250 && hash[21]==161 && hash[22]==158 && hash[23]==170 && hash[24]==75 && hash[25]==152 && hash[26]==145 && hash[27]==79 && hash[28]==95 && hash[29]==74 && hash[30]==54 && hash[31]==176)
            {
                return true;
            }
            else if(hash[0]==122 && hash[1]==150 && hash[2]==4 && hash[3]==162 && hash[4]==214 && hash[5]==37 && hash[6]==190 && hash[7]==186 && hash[8]==127 && hash[9]==147 && hash[10]==244 && hash[11]==138 && hash[12]==37 && hash[13]==201 && hash[14]==123 && hash[15]==58 && hash[16]==5 && hash[17]==248 && hash[18]==146 && hash[19]==0 && hash[20]==42 && hash[21]==174 && hash[22]==19 && hash[23]==161 && hash[24]==73 && hash[25]==93 && hash[26]==4 && hash[27]==240 && hash[28]==160 && hash[29]==40 && hash[30]==35 && hash[31]==133)
            {
                return true;
            }
            else if(hash[0]==124 && hash[1]==234 && hash[2]==109 && hash[3]==136 && hash[4]==143 && hash[5]==115 && hash[6]==72 && hash[7]==92 && hash[8]==255 && hash[9]==47 && hash[10]==219 && hash[11]==57 && hash[12]==73 && hash[13]==195 && hash[14]==123 && hash[15]==252 && hash[16]==110 && hash[17]==123 && hash[18]==186 && hash[19]==127 && hash[20]==255 && hash[21]==87 && hash[22]==48 && hash[23]==81 && hash[24]==218 && hash[25]==144 && hash[26]==1 && hash[27]==200 && hash[28]==7 && hash[29]==23 && hash[30]==193 && hash[31]==44)
            {
                return true;
            }
            else if(hash[0]==126 && hash[1]==194 && hash[2]==78 && hash[3]==49 && hash[4]==90 && hash[5]==86 && hash[6]==35 && hash[7]==146 && hash[8]==6 && hash[9]==70 && hash[10]==93 && hash[11]==81 && hash[12]==116 && hash[13]==107 && hash[14]==154 && hash[15]==44 && hash[16]==78 && hash[17]==11 && hash[18]==122 && hash[19]==22 && hash[20]==146 && hash[21]==186 && hash[22]==166 && hash[23]==210 && hash[24]==52 && hash[25]==229 && hash[26]==99 && hash[27]==228 && hash[28]==225 && hash[29]==248 && hash[30]==62 && hash[31]==160)
            {
                return true;
            }
            else if(hash[0]==127 && hash[1]==154 && hash[2]==254 && hash[3]==248 && hash[4]==32 && hash[5]==239 && hash[6]==218 && hash[7]==109 && hash[8]==82 && hash[9]==234 && hash[10]==195 && hash[11]==225 && hash[12]==60 && hash[13]==10 && hash[14]==162 && hash[15]==133 && hash[16]==96 && hash[17]==209 && hash[18]==243 && hash[19]==18 && hash[20]==192 && hash[21]==219 && hash[22]==15 && hash[23]==194 && hash[24]==31 && hash[25]==156 && hash[26]==158 && hash[27]==27 && hash[28]==215 && hash[29]==211 && hash[30]==4 && hash[31]==129)
            {
                return true;
            }
            else if(hash[0]==129 && hash[1]==126 && hash[2]==69 && hash[3]==249 && hash[4]==140 && hash[5]==15 && hash[6]==230 && hash[7]==123 && hash[8]==50 && hash[9]==228 && hash[10]==191 && hash[11]==131 && hash[12]==205 && hash[13]==16 && hash[14]==216 && hash[15]==137 && hash[16]==231 && hash[17]==105 && hash[18]==94 && hash[19]==129 && hash[20]==47 && hash[21]==155 && hash[22]==68 && hash[23]==14 && hash[24]==139 && hash[25]==217 && hash[26]==210 && hash[27]==88 && hash[28]==7 && hash[29]==109 && hash[30]==122 && hash[31]==199)
            {
                return true;
            }
            else if(hash[0]==141 && hash[1]==230 && hash[2]==140 && hash[3]==252 && hash[4]==116 && hash[5]==157 && hash[6]==204 && hash[7]==114 && hash[8]==227 && hash[9]==7 && hash[10]==166 && hash[11]==246 && hash[12]==179 && hash[13]==115 && hash[14]==13 && hash[15]==99 && hash[16]==71 && hash[17]==115 && hash[18]==28 && hash[19]==163 && hash[20]==23 && hash[21]==40 && hash[22]==235 && hash[23]==107 && hash[24]==98 && hash[25]==225 && hash[26]==200 && hash[27]==95 && hash[28]==205 && hash[29]==111 && hash[30]==94 && hash[31]==60)
            {
                return true;
            }
            else if(hash[0]==145 && hash[1]==105 && hash[2]==60 && hash[3]==114 && hash[4]==217 && hash[5]==82 && hash[6]==230 && hash[7]==100 && hash[8]==182 && hash[9]==118 && hash[10]==47 && hash[11]==88 && hash[12]==45 && hash[13]==231 && hash[14]==138 && hash[15]==176 && hash[16]==142 && hash[17]==241 && hash[18]==42 && hash[19]==161 && hash[20]==248 && hash[21]==103 && hash[22]==6 && hash[23]==201 && hash[24]==233 && hash[25]==165 && hash[26]==17 && hash[27]==23 && hash[28]==24 && hash[29]==238 && hash[30]==139 && hash[31]==6)
            {
                return true;
            }
            else if(hash[0]==146 && hash[1]==49 && hash[2]==54 && hash[3]==27 && hash[4]==148 && hash[5]==183 && hash[6]==194 && hash[7]==75 && hash[8]==85 && hash[9]==98 && hash[10]==141 && hash[11]==70 && hash[12]==223 && hash[13]==171 && hash[14]==213 && hash[15]==77 && hash[16]==232 && hash[17]==135 && hash[18]==47 && hash[19]==32 && hash[20]==161 && hash[21]==54 && hash[22]==41 && hash[23]==196 && hash[24]==126 && hash[25]==107 && hash[26]==90 && hash[27]==44 && hash[28]==23 && hash[29]==191 && hash[30]==112 && hash[31]==242)
            {
                return true;
            }
            else if(hash[0]==150 && hash[1]==101 && hash[2]==161 && hash[3]==143 && hash[4]==106 && hash[5]==101 && hash[6]==230 && hash[7]==19 && hash[8]==34 && hash[9]==232 && hash[10]==241 && hash[11]==65 && hash[12]==63 && hash[13]==188 && hash[14]==228 && hash[15]==247 && hash[16]==167 && hash[17]==183 && hash[18]==216 && hash[19]==25 && hash[20]==62 && hash[21]==178 && hash[22]==171 && hash[23]==114 && hash[24]==244 && hash[25]==195 && hash[26]==132 && hash[27]==133 && hash[28]==78 && hash[29]==128 && hash[30]==236 && hash[31]==95)
            {
                return true;
            }
            else if(hash[0]==151 && hash[1]==194 && hash[2]==152 && hash[3]==89 && hash[4]==3 && hash[5]==63 && hash[6]==221 && hash[7]==117 && hash[8]==246 && hash[9]==36 && hash[10]==135 && hash[11]==190 && hash[12]==218 && hash[13]==235 && hash[14]==150 && hash[15]==234 && hash[16]==198 && hash[17]==255 && hash[18]==188 && hash[19]==245 && hash[20]==173 && hash[21]==107 && hash[22]==0 && hash[23]==229 && hash[24]==145 && hash[25]==49 && hash[26]==123 && hash[27]==27 && hash[28]==94 && hash[29]==189 && hash[30]==206 && hash[31]==8)
            {
                return true;
            }
            else if(hash[0]==154 && hash[1]==141 && hash[2]==180 && hash[3]==127 && hash[4]==219 && hash[5]==180 && hash[6]==176 && hash[7]==128 && hash[8]==45 && hash[9]==214 && hash[10]==72 && hash[11]==244 && hash[12]==239 && hash[13]==171 && hash[14]==230 && hash[15]==27 && hash[16]==133 && hash[17]==2 && hash[18]==97 && hash[19]==79 && hash[20]==232 && hash[21]==110 && hash[22]==151 && hash[23]==160 && hash[24]==124 && hash[25]==253 && hash[26]==26 && hash[27]==230 && hash[28]==241 && hash[29]==158 && hash[30]==209 && hash[31]==131)
            {
                return true;
            }
            else if(hash[0]==155 && hash[1]==130 && hash[2]==172 && hash[3]==15 && hash[4]==16 && hash[5]==183 && hash[6]==220 && hash[7]==17 && hash[8]==141 && hash[9]==67 && hash[10]==155 && hash[11]==253 && hash[12]==145 && hash[13]==220 && hash[14]==163 && hash[15]==156 && hash[16]==202 && hash[17]==22 && hash[18]==95 && hash[19]==132 && hash[20]==25 && hash[21]==79 && hash[22]==168 && hash[23]==72 && hash[24]==85 && hash[25]==153 && hash[26]==205 && hash[27]==235 && hash[28]==98 && hash[29]==35 && hash[30]==103 && hash[31]==20)
            {
                return true;
            }
            else if(hash[0]==158 && hash[1]==115 && hash[2]==178 && hash[3]==240 && hash[4]==164 && hash[5]==1 && hash[6]==254 && hash[7]==103 && hash[8]==9 && hash[9]==20 && hash[10]==34 && hash[11]==172 && hash[12]==131 && hash[13]==48 && hash[14]==213 && hash[15]==19 && hash[16]==139 && hash[17]==198 && hash[18]==93 && hash[19]==175 && hash[20]==36 && hash[21]==228 && hash[22]==53 && hash[23]==100 && hash[24]==153 && hash[25]==44 && hash[26]==152 && hash[27]==1 && hash[28]==249 && hash[29]==226 && hash[30]==103 && hash[31]==27)
            {
                return true;
            }
            else if(hash[0]==159)
            {
                if(hash[1]==156 && hash[2]==252 && hash[3]==178 && hash[4]==16 && hash[5]==34 && hash[6]==128 && hash[7]==155 && hash[8]==68 && hash[9]==207 && hash[10]==220 && hash[11]==78 && hash[12]==179 && hash[13]==39 && hash[14]==157 && hash[15]==47 && hash[16]==110 && hash[17]==31 && hash[18]==100 && hash[19]==87 && hash[20]==252 && hash[21]==171 && hash[22]==112 && hash[23]==158 && hash[24]==111 && hash[25]==79 && hash[26]==224 && hash[27]==115 && hash[28]==96 && hash[29]==136 && hash[30]==128 && hash[31]==143)
                {
                    return true;
                }
                else if(hash[1]==245 && hash[2]==186 && hash[3]==220 && hash[4]==126 && hash[5]==40 && hash[6]==248 && hash[7]==149 && hash[8]==5 && hash[9]==1 && hash[10]==249 && hash[11]==253 && hash[12]==215 && hash[13]==249 && hash[14]==80 && hash[15]==137 && hash[16]==83 && hash[17]==45 && hash[18]==108 && hash[19]==215 && hash[20]==137 && hash[21]==71 && hash[22]==127 && hash[23]==227 && hash[24]==185 && hash[25]==159 && hash[26]==39 && hash[27]==252 && hash[28]==215 && hash[29]==216 && hash[30]==31 && hash[31]==142)
                {
                    return true;
                }
                else if(hash[1]==55 && hash[2]==252 && hash[3]==152 && hash[4]==101 && hash[5]==45 && hash[6]==166 && hash[7]==94 && hash[8]==38 && hash[9]==100 && hash[10]==5 && hash[11]==170 && hash[12]==193 && hash[13]==5 && hash[14]==37 && hash[15]==63 && hash[16]==168 && hash[17]==149 && hash[18]==217 && hash[19]==49 && hash[20]==31 && hash[21]==211 && hash[22]==74 && hash[23]==188 && hash[24]==132 && hash[25]==237 && hash[26]==1 && hash[27]==73 && hash[28]==142 && hash[29]==156 && hash[30]==226 && hash[31]==143)
                {
                    return true;
                }
            }
            else if(hash[0]==160 && hash[1]==13 && hash[2]==47 && hash[3]==153 && hash[4]==37 && hash[5]==44 && hash[6]==90 && hash[7]==163 && hash[8]==151 && hash[9]==126 && hash[10]==127 && hash[11]==126 && hash[12]==192 && hash[13]==228 && hash[14]==160 && hash[15]==93 && hash[16]==60 && hash[17]==237 && hash[18]==144 && hash[19]==196 && hash[20]==222 && hash[21]==152 && hash[22]==226 && hash[23]==32 && hash[24]==117 && hash[25]==245 && hash[26]==233 && hash[27]==84 && hash[28]==200 && hash[29]==137 && hash[30]==189 && hash[31]==96)
            {
                return true;
            }
            else if(hash[0]==162 && hash[1]==167 && hash[2]==200 && hash[3]==83 && hash[4]==192 && hash[5]==41 && hash[6]==144 && hash[7]==229 && hash[8]==160 && hash[9]==3 && hash[10]==128 && hash[11]==253 && hash[12]==52 && hash[13]==183 && hash[14]==45 && hash[15]==15 && hash[16]==80 && hash[17]==56 && hash[18]==226 && hash[19]==114 && hash[20]==227 && hash[21]==192 && hash[22]==11 && hash[23]==179 && hash[24]==18 && hash[25]==164 && hash[26]==73 && hash[27]==9 && hash[28]==18 && hash[29]==48 && hash[30]==129 && hash[31]==177)
            {
                return true;
            }
            else if(hash[0]==164)
            {
                if(hash[1]==225 && hash[2]==63 && hash[3]==221 && hash[4]==215 && hash[5]==135 && hash[6]==133 && hash[7]==133 && hash[8]==1 && hash[9]==213 && hash[10]==67 && hash[11]==246 && hash[12]==58 && hash[13]==0 && hash[14]==57 && hash[15]==28 && hash[16]==11 && hash[17]==131 && hash[18]==166 && hash[19]==67 && hash[20]==228 && hash[21]==1 && hash[22]==106 && hash[23]==37 && hash[24]==176 && hash[25]==102 && hash[26]==181 && hash[27]==131 && hash[28]==89 && hash[29]==23 && hash[30]==100 && hash[31]==185)
                {
                    return true;
                }
                else if(hash[1]==67 && hash[2]==217 && hash[3]==253 && hash[4]==16 && hash[5]==10 && hash[6]==197 && hash[7]==238 && hash[8]==85 && hash[9]==235 && hash[10]==57 && hash[11]==180 && hash[12]==170 && hash[13]==179 && hash[14]==208 && hash[15]==195 && hash[16]==105 && hash[17]==187 && hash[18]==118 && hash[19]==133 && hash[20]==121 && hash[21]==65 && hash[22]==239 && hash[23]==220 && hash[24]==178 && hash[25]==243 && hash[26]==56 && hash[27]==90 && hash[28]==246 && hash[29]==28 && hash[30]==61 && hash[31]==173)
                {
                    return true;
                }
            }
            else if(hash[0]==178 && hash[1]==198 && hash[2]==49 && hash[3]==87 && hash[4]==88 && hash[5]==99 && hash[6]==48 && hash[7]==100 && hash[8]==194 && hash[9]==4 && hash[10]==54 && hash[11]==84 && hash[12]==33 && hash[13]==182 && hash[14]==84 && hash[15]==236 && hash[16]==20 && hash[17]==148 && hash[18]==5 && hash[19]==80 && hash[20]==139 && hash[21]==61 && hash[22]==163 && hash[23]==176 && hash[24]==108 && hash[25]==18 && hash[26]==168 && hash[27]==204 && hash[28]==8 && hash[29]==12 && hash[30]==10 && hash[31]==130)
            {
                return true;
            }
            else if(hash[0]==18 && hash[1]==90 && hash[2]==221 && hash[3]==109 && hash[4]==34 && hash[5]==52 && hash[6]==165 && hash[7]==38 && hash[8]==142 && hash[9]==182 && hash[10]==67 && hash[11]==164 && hash[12]==170 && hash[13]==40 && hash[14]==168 && hash[15]==155 && hash[16]==82 && hash[17]==32 && hash[18]==78 && hash[19]==144 && hash[20]==43 && hash[21]==63 && hash[22]==40 && hash[23]==179 && hash[24]==206 && hash[25]==163 && hash[26]==83 && hash[27]==214 && hash[28]==233 && hash[29]==48 && hash[30]==13 && hash[31]==254)
            {
                return true;
            }
            else if(hash[0]==182 && hash[1]==98 && hash[2]==247 && hash[3]==122 && hash[4]==207 && hash[5]==139 && hash[6]==58 && hash[7]==106 && hash[8]==3 && hash[9]==217 && hash[10]==190 && hash[11]==40 && hash[12]==175 && hash[13]==195 && hash[14]==108 && hash[15]==248 && hash[16]==186 && hash[17]==96 && hash[18]==66 && hash[19]==49 && hash[20]==11 && hash[21]==126 && hash[22]==36 && hash[23]==128 && hash[24]==203 && hash[25]==45 && hash[26]==192 && hash[27]==250 && hash[28]==247 && hash[29]==5 && hash[30]==16 && hash[31]==117)
            {
                return true;
            }
            else if(hash[0]==184 && hash[1]==140 && hash[2]==105 && hash[3]==118 && hash[4]==159 && hash[5]==197 && hash[6]==35 && hash[7]==4 && hash[8]==144 && hash[9]==239 && hash[10]==212 && hash[11]==240 && hash[12]==124 && hash[13]==196 && hash[14]==214 && hash[15]==2 && hash[16]==152 && hash[17]==10 && hash[18]==170 && hash[19]==68 && hash[20]==200 && hash[21]==204 && hash[22]==110 && hash[23]==56 && hash[24]==202 && hash[25]==241 && hash[26]==151 && hash[27]==80 && hash[28]==63 && hash[29]==14 && hash[30]==138 && hash[31]==87)
            {
                return true;
            }
            else if(hash[0]==188 && hash[1]==82 && hash[2]==31 && hash[3]==74 && hash[4]==65 && hash[5]==204 && hash[6]==20 && hash[7]==2 && hash[8]==229 && hash[9]==32 && hash[10]==4 && hash[11]==170 && hash[12]==128 && hash[13]==32 && hash[14]==26 && hash[15]==195 && hash[16]==185 && hash[17]==34 && hash[18]==48 && hash[19]==207 && hash[20]==172 && hash[21]==19 && hash[22]==220 && hash[23]==59 && hash[24]==129 && hash[25]==17 && hash[26]==121 && hash[27]==140 && hash[28]==173 && hash[29]==206 && hash[30]==22 && hash[31]==199)
            {
                return true;
            }
            else if(hash[0]==190)
            {
                if(hash[1]==189 && hash[2]==202 && hash[3]==220 && hash[4]==58 && hash[5]==75 && hash[6]==93 && hash[7]==215 && hash[8]==142 && hash[9]==14 && hash[10]==147 && hash[11]==37 && hash[12]==190 && hash[13]==60 && hash[14]==168 && hash[15]==218 && hash[16]==249 && hash[17]==108 && hash[18]==143 && hash[19]==9 && hash[20]==52 && hash[21]==162 && hash[22]==19 && hash[23]==45 && hash[24]==237 && hash[25]==240 && hash[26]==182 && hash[27]==242 && hash[28]==103 && hash[29]==103 && hash[30]==107 && hash[31]==65)
                {
                    return true;
                }
                else if(hash[1]==200 && hash[2]==202 && hash[3]==196 && hash[4]==20 && hash[5]==30 && hash[6]==200 && hash[7]==232 && hash[8]==177 && hash[9]==202 && hash[10]==56 && hash[11]==124 && hash[12]==15 && hash[13]==108 && hash[14]==135 && hash[15]==204 && hash[16]==116 && hash[17]==178 && hash[18]==90 && hash[19]==71 && hash[20]==232 && hash[21]==98 && hash[22]==152 && hash[23]==82 && hash[24]==121 && hash[25]==35 && hash[26]==114 && hash[27]==26 && hash[28]==179 && hash[29]==228 && hash[30]==245 && hash[31]==40)
                {
                    return true;
                }
                else if(hash[1]==24 && hash[2]==140 && hash[3]==196 && hash[4]==194 && hash[5]==105 && hash[6]==40 && hash[7]==41 && hash[8]==74 && hash[9]==89 && hash[10]==63 && hash[11]==188 && hash[12]==17 && hash[13]==141 && hash[14]==142 && hash[15]==111 && hash[16]==40 && hash[17]==124 && hash[18]==89 && hash[19]==24 && hash[20]==215 && hash[21]==91 && hash[22]==209 && hash[23]==85 && hash[24]==187 && hash[25]==200 && hash[26]==70 && hash[27]==12 && hash[28]==175 && hash[29]==26 && hash[30]==206 && hash[31]==25)
                {
                    return true;
                }
            }
            else if(hash[0]==193 && hash[1]==250 && hash[2]==37 && hash[3]==82 && hash[4]==155 && hash[5]==234 && hash[6]==22 && hash[7]==4 && hash[8]==103 && hash[9]==132 && hash[10]==26 && hash[11]==158 && hash[12]==113 && hash[13]==35 && hash[14]==16 && hash[15]==109 && hash[16]==236 && hash[17]==235 && hash[18]==204 && hash[19]==13 && hash[20]==181 && hash[21]==239 && hash[22]==36 && hash[23]==186 && hash[24]==218 && hash[25]==201 && hash[26]==192 && hash[27]==11 && hash[28]==152 && hash[29]==18 && hash[30]==72 && hash[31]==243)
            {
                return true;
            }
            else if(hash[0]==196 && hash[1]==210 && hash[2]==187 && hash[3]==85 && hash[4]==106 && hash[5]==30 && hash[6]==93 && hash[7]==4 && hash[8]==163 && hash[9]==128 && hash[10]==21 && hash[11]==75 && hash[12]==62 && hash[13]==106 && hash[14]==251 && hash[15]==245 && hash[16]==170 && hash[17]==156 && hash[18]==146 && hash[19]==184 && hash[20]==153 && hash[21]==147 && hash[22]==215 && hash[23]==119 && hash[24]==169 && hash[25]==136 && hash[26]==249 && hash[27]==107 && hash[28]==121 && hash[29]==183 && hash[30]==199 && hash[31]==177)
            {
                return true;
            }
            else if(hash[0]==198)
            {
                if(hash[1]==204 && hash[2]==161 && hash[3]==244 && hash[4]==108 && hash[5]==58 && hash[6]==173 && hash[7]==221 && hash[8]==249 && hash[9]==25 && hash[10]==26 && hash[11]==26 && hash[12]==30 && hash[13]==169 && hash[14]==150 && hash[15]==174 && hash[16]==141 && hash[17]==223 && hash[18]==23 && hash[19]==230 && hash[20]==214 && hash[21]==215 && hash[22]==110 && hash[23]==172 && hash[24]==18 && hash[25]==156 && hash[26]==28 && hash[27]==121 && hash[28]==143 && hash[29]==214 && hash[30]==196 && hash[31]==109)
                {
                    return true;
                }
                else if(hash[1]==226 && hash[2]==120 && hash[3]==173 && hash[4]==28 && hash[5]==29 && hash[6]==230 && hash[7]==22 && hash[8]==196 && hash[9]==0 && hash[10]==198 && hash[11]==147 && hash[12]==58 && hash[13]==143 && hash[14]==105 && hash[15]==152 && hash[16]==130 && hash[17]==16 && hash[18]==71 && hash[19]==92 && hash[20]==26 && hash[21]==177 && hash[22]==22 && hash[23]==43 && hash[24]==191 && hash[25]==145 && hash[26]==160 && hash[27]==2 && hash[28]==250 && hash[29]==119 && hash[30]==224 && hash[31]==117)
                {
                    return true;
                }
                else if(hash[1]==48 && hash[2]==160 && hash[3]==67 && hash[4]==140 && hash[5]==15 && hash[6]==173 && hash[7]==120 && hash[8]==171 && hash[9]==235 && hash[10]==3 && hash[11]==66 && hash[12]==49 && hash[13]==20 && hash[14]==196 && hash[15]==182 && hash[16]==46 && hash[17]==25 && hash[18]==97 && hash[19]==22 && hash[20]==55 && hash[21]==198 && hash[22]==30 && hash[23]==222 && hash[24]==142 && hash[25]==22 && hash[26]==219 && hash[27]==108 && hash[28]==235 && hash[29]==199 && hash[30]==40 && hash[31]==250)
                {
                    return true;
                }
            }
            else if(hash[0]==199 && hash[1]==114 && hash[2]==218 && hash[3]==246 && hash[4]==9 && hash[5]==243 && hash[6]==110 && hash[7]==6 && hash[8]==160 && hash[9]==211 && hash[10]==27 && hash[11]==168 && hash[12]==177 && hash[13]==192 && hash[14]==87 && hash[15]==158 && hash[16]==222 && hash[17]==250 && hash[18]==21 && hash[19]==138 && hash[20]==203 && hash[21]==82 && hash[22]==70 && hash[23]==49 && hash[24]==248 && hash[25]==200 && hash[26]==203 && hash[27]==153 && hash[28]==28 && hash[29]==216 && hash[30]==112 && hash[31]==3)
            {
                return true;
            }
            else if(hash[0]==2 && hash[1]==69 && hash[2]==138 && hash[3]==34 && hash[4]==163 && hash[5]==69 && hash[6]==251 && hash[7]==96 && hash[8]==184 && hash[9]==91 && hash[10]==1 && hash[11]==180 && hash[12]==68 && hash[13]==214 && hash[14]==77 && hash[15]==244 && hash[16]==244 && hash[17]==173 && hash[18]==165 && hash[19]==94 && hash[20]==187 && hash[21]==161 && hash[22]==51 && hash[23]==224 && hash[24]==183 && hash[25]==215 && hash[26]==104 && hash[27]==187 && hash[28]==103 && hash[29]==49 && hash[30]==159 && hash[31]==98)
            {
                return true;
            }
            else if(hash[0]==200 && hash[1]==94 && hash[2]==8 && hash[3]==177 && hash[4]==182 && hash[5]==220 && hash[6]==237 && hash[7]==28 && hash[8]==251 && hash[9]==228 && hash[10]==176 && hash[11]==114 && hash[12]==125 && hash[13]==190 && hash[14]==227 && hash[15]==168 && hash[16]==122 && hash[17]==218 && hash[18]==91 && hash[19]==79 && hash[20]==251 && hash[21]==130 && hash[22]==18 && hash[23]==85 && hash[24]==25 && hash[25]==75 && hash[26]==61 && hash[27]==65 && hash[28]==55 && hash[29]==139 && hash[30]==97 && hash[31]==132)
            {
                return true;
            }
            else if(hash[0]==201)
            {
                if(hash[1]==211 && hash[2]==11 && hash[3]==197 && hash[4]==242 && hash[5]==184 && hash[6]==111 && hash[7]==57 && hash[8]==120 && hash[9]==137 && hash[10]==158 && hash[11]==155 && hash[12]==174 && hash[13]==6 && hash[14]==22 && hash[15]==127 && hash[16]==101 && hash[17]==174 && hash[18]==177 && hash[19]==60 && hash[20]==177 && hash[21]==33 && hash[22]==153 && hash[23]==117 && hash[24]==227 && hash[25]==157 && hash[26]==129 && hash[27]==134 && hash[28]==135 && hash[29]==199 && hash[30]==183 && hash[31]==106)
                {
                    return true;
                }
                else if(hash[1]==56 && hash[2]==187 && hash[3]==164 && hash[4]==99 && hash[5]==207 && hash[6]==69 && hash[7]==85 && hash[8]==74 && hash[9]==101 && hash[10]==77 && hash[11]==43 && hash[12]==152 && hash[13]==234 && hash[14]==100 && hash[15]==20 && hash[16]==37 && hash[17]==178 && hash[18]==220 && hash[19]==149 && hash[20]==207 && hash[21]==148 && hash[22]==177 && hash[23]==131 && hash[24]==129 && hash[25]==187 && hash[26]==144 && hash[27]==159 && hash[28]==68 && hash[29]==76 && hash[30]==152 && hash[31]==79)
                {
                    return true;
                }
                else if(hash[1]==81 && hash[2]==218 && hash[3]==52 && hash[4]==8 && hash[5]==145 && hash[6]==254 && hash[7]==14 && hash[8]==185 && hash[9]==110 && hash[10]==204 && hash[11]==141 && hash[12]==159 && hash[13]==97 && hash[14]==221 && hash[15]==126 && hash[16]==238 && hash[17]==214 && hash[18]==241 && hash[19]==161 && hash[20]==246 && hash[21]==164 && hash[22]==211 && hash[23]==151 && hash[24]==77 && hash[25]==72 && hash[26]==80 && hash[27]==112 && hash[28]==211 && hash[29]==35 && hash[30]==5 && hash[31]==47)
                {
                    return true;
                }
            }
            else if(hash[0]==202 && hash[1]==22 && hash[2]==110 && hash[3]==152 && hash[4]==37 && hash[5]==119 && hash[6]==25 && hash[7]==145 && hash[8]==168 && hash[9]==247 && hash[10]==49 && hash[11]==93 && hash[12]==204 && hash[13]==11 && hash[14]==113 && hash[15]==32 && hash[16]==141 && hash[17]==189 && hash[18]==227 && hash[19]==116 && hash[20]==51 && hash[21]==116 && hash[22]==203 && hash[23]==7 && hash[24]==33 && hash[25]==2 && hash[26]==202 && hash[27]==131 && hash[28]==8 && hash[29]==9 && hash[30]==98 && hash[31]==244)
            {
                return true;
            }
            else if(hash[0]==203 && hash[1]==116 && hash[2]==133 && hash[3]==205 && hash[4]==38 && hash[5]==133 && hash[6]==9 && hash[7]==120 && hash[8]==94 && hash[9]==106 && hash[10]==150 && hash[11]==125 && hash[12]==40 && hash[13]==125 && hash[14]==118 && hash[15]==211 && hash[16]==4 && hash[17]==181 && hash[18]==228 && hash[19]==66 && hash[20]==118 && hash[21]==255 && hash[22]==215 && hash[23]==161 && hash[24]==229 && hash[25]==38 && hash[26]==154 && hash[27]==107 && hash[28]==252 && hash[29]==64 && hash[30]==111 && hash[31]==249)
            {
                return true;
            }
            else if(hash[0]==204 && hash[1]==77 && hash[2]==0 && hash[3]==235 && hash[4]==130 && hash[5]==25 && hash[6]==147 && hash[7]==210 && hash[8]==41 && hash[9]==61 && hash[10]==247 && hash[11]==238 && hash[12]==165 && hash[13]==92 && hash[14]==83 && hash[15]==179 && hash[16]==84 && hash[17]==254 && hash[18]==18 && hash[19]==74 && hash[20]==10 && hash[21]==0 && hash[22]==78 && hash[23]==42 && hash[24]==94 && hash[25]==103 && hash[26]==157 && hash[27]==217 && hash[28]==76 && hash[29]==240 && hash[30]==242 && hash[31]==62)
            {
                return true;
            }
            else if(hash[0]==205 && hash[1]==47 && hash[2]==90 && hash[3]==7 && hash[4]==26 && hash[5]==104 && hash[6]==28 && hash[7]==107 && hash[8]==214 && hash[9]==183 && hash[10]==86 && hash[11]==246 && hash[12]==50 && hash[13]==126 && hash[14]==193 && hash[15]==195 && hash[16]==144 && hash[17]==151 && hash[18]==193 && hash[19]==227 && hash[20]==115 && hash[21]==167 && hash[22]==218 && hash[23]==230 && hash[24]==62 && hash[25]==208 && hash[26]==29 && hash[27]==187 && hash[28]==137 && hash[29]==143 && hash[30]==121 && hash[31]==40)
            {
                return true;
            }
            else if(hash[0]==207 && hash[1]==195 && hash[2]==1 && hash[3]==223 && hash[4]==59 && hash[5]==73 && hash[6]==131 && hash[7]==130 && hash[8]==194 && hash[9]==159 && hash[10]==89 && hash[11]==70 && hash[12]==145 && hash[13]==239 && hash[14]==193 && hash[15]==152 && hash[16]==170 && hash[17]==217 && hash[18]==38 && hash[19]==174 && hash[20]==37 && hash[21]==105 && hash[22]==167 && hash[23]==236 && hash[24]==243 && hash[25]==30 && hash[26]==36 && hash[27]==208 && hash[28]==229 && hash[29]==187 && hash[30]==131 && hash[31]==124)
            {
                return true;
            }
            else if(hash[0]==208)
            {
                if(hash[1]==100 && hash[2]==92 && hash[3]==34 && hash[4]==192 && hash[5]==15 && hash[6]==73 && hash[7]==162 && hash[8]==186 && hash[9]==2 && hash[10]==185 && hash[11]==40 && hash[12]==33 && hash[13]==244 && hash[14]==19 && hash[15]==9 && hash[16]==243 && hash[17]==172 && hash[18]==181 && hash[19]==92 && hash[20]==212 && hash[21]==47 && hash[22]==147 && hash[23]==102 && hash[24]==65 && hash[25]==138 && hash[26]==228 && hash[27]==141 && hash[28]==210 && hash[29]==80 && hash[30]==147 && hash[31]==133)
                {
                    return true;
                }
                else if(hash[1]==24 && hash[2]==40 && hash[3]==210 && hash[4]==211 && hash[5]==68 && hash[6]==174 && hash[7]==220 && hash[8]==47 && hash[9]==174 && hash[10]==15 && hash[11]==199 && hash[12]==56 && hash[13]==19 && hash[14]==162 && hash[15]==215 && hash[16]==48 && hash[17]==127 && hash[18]==180 && hash[19]==175 && hash[20]==158 && hash[21]==216 && hash[22]==234 && hash[23]==186 && hash[24]==42 && hash[25]==157 && hash[26]==162 && hash[27]==28 && hash[28]==101 && hash[29]==5 && hash[30]==195 && hash[31]==25)
                {
                    return true;
                }
            }
            else if(hash[0]==210 && hash[1]==32 && hash[2]==220 && hash[3]==97 && hash[4]==146 && hash[5]==156 && hash[6]==19 && hash[7]==252 && hash[8]==182 && hash[9]==170 && hash[10]==49 && hash[11]==1 && hash[12]==221 && hash[13]==3 && hash[14]==66 && hash[15]==125 && hash[16]==204 && hash[17]==222 && hash[18]==107 && hash[19]==241 && hash[20]==105 && hash[21]==173 && hash[22]==162 && hash[23]==22 && hash[24]==154 && hash[25]==137 && hash[26]==215 && hash[27]==63 && hash[28]==119 && hash[29]==249 && hash[30]==116 && hash[31]==133)
            {
                return true;
            }
            else if(hash[0]==211)
            {
                if(hash[1]==190 && hash[2]==23 && hash[3]==191 && hash[4]==49 && hash[5]==57 && hash[6]==13 && hash[7]==61 && hash[8]==151 && hash[9]==168 && hash[10]==88 && hash[11]==225 && hash[12]==181 && hash[13]==254 && hash[14]==106 && hash[15]==160 && hash[16]==13 && hash[17]==212 && hash[18]==81 && hash[19]==221 && hash[20]==133 && hash[21]==233 && hash[22]==231 && hash[23]==159 && hash[24]==164 && hash[25]==60 && hash[26]==185 && hash[27]==21 && hash[28]==240 && hash[29]==13 && hash[30]==71 && hash[31]==216)
                {
                    return true;
                }
                else if(hash[1]==250 && hash[2]==113 && hash[3]==187 && hash[4]==45 && hash[5]==162 && hash[6]==86 && hash[7]==18 && hash[8]==60 && hash[9]==37 && hash[10]==193 && hash[11]==118 && hash[12]==94 && hash[13]==129 && hash[14]==96 && hash[15]==198 && hash[16]==116 && hash[17]==70 && hash[18]==23 && hash[19]==235 && hash[20]==21 && hash[21]==28 && hash[22]==211 && hash[23]==244 && hash[24]==158 && hash[25]==184 && hash[26]==73 && hash[27]==107 && hash[28]==66 && hash[29]==54 && hash[30]==206 && hash[31]==11)
                {
                    return true;
                }
            }
            else if(hash[0]==215 && hash[1]==163 && hash[2]==17 && hash[3]==148 && hash[4]==64 && hash[5]==50 && hash[6]==28 && hash[7]==108 && hash[8]==179 && hash[9]==251 && hash[10]==13 && hash[11]==114 && hash[12]==81 && hash[13]==134 && hash[14]==70 && hash[15]==23 && hash[16]==96 && hash[17]==137 && hash[18]==206 && hash[19]==139 && hash[20]==187 && hash[21]==34 && hash[22]==37 && hash[23]==180 && hash[24]==227 && hash[25]==61 && hash[26]==178 && hash[27]==98 && hash[28]==127 && hash[29]==34 && hash[30]==19 && hash[31]==91)
            {
                return true;
            }
            else if(hash[0]==217 && hash[1]==214 && hash[2]==155 && hash[3]==180 && hash[4]==13 && hash[5]==121 && hash[6]==86 && hash[7]==90 && hash[8]==213 && hash[9]==173 && hash[10]==219 && hash[11]==47 && hash[12]==16 && hash[13]==84 && hash[14]==230 && hash[15]==154 && hash[16]==181 && hash[17]==246 && hash[18]==44 && hash[19]==10 && hash[20]==233 && hash[21]==111 && hash[22]==81 && hash[23]==13 && hash[24]==113 && hash[25]==74 && hash[26]==132 && hash[27]==35 && hash[28]==57 && hash[29]==178 && hash[30]==161 && hash[31]==23)
            {
                return true;
            }
            else if(hash[0]==219 && hash[1]==236 && hash[2]==52 && hash[3]==166 && hash[4]==34 && hash[5]==79 && hash[6]==40 && hash[7]==184 && hash[8]==22 && hash[9]==119 && hash[10]==88 && hash[11]==16 && hash[12]==47 && hash[13]==97 && hash[14]==193 && hash[15]==203 && hash[16]==164 && hash[17]==209 && hash[18]==19 && hash[19]==132 && hash[20]==42 && hash[21]==241 && hash[22]==62 && hash[23]==147 && hash[24]==96 && hash[25]==24 && hash[26]==101 && hash[27]==242 && hash[28]==194 && hash[29]==166 && hash[30]==10 && hash[31]==240)
            {
                return true;
            }
            else if(hash[0]==222 && hash[1]==60 && hash[2]==68 && hash[3]==218 && hash[4]==212 && hash[5]==31 && hash[6]==109 && hash[7]==67 && hash[8]==97 && hash[9]==43 && hash[10]==3 && hash[11]==158 && hash[12]==45 && hash[13]==107 && hash[14]==205 && hash[15]==193 && hash[16]==248 && hash[17]==122 && hash[18]==148 && hash[19]==48 && hash[20]==119 && hash[21]==194 && hash[22]==245 && hash[23]==14 && hash[24]==178 && hash[25]==232 && hash[26]==139 && hash[27]==94 && hash[28]==107 && hash[29]==62 && hash[30]==247 && hash[31]==251)
            {
                return true;
            }
            else if(hash[0]==224 && hash[1]==141 && hash[2]==209 && hash[3]==187 && hash[4]==210 && hash[5]==207 && hash[6]==61 && hash[7]==230 && hash[8]==109 && hash[9]==9 && hash[10]==247 && hash[11]==187 && hash[12]==148 && hash[13]==235 && hash[14]==79 && hash[15]==98 && hash[16]==137 && hash[17]==45 && hash[18]==3 && hash[19]==137 && hash[20]==44 && hash[21]==92 && hash[22]==114 && hash[23]==8 && hash[24]==177 && hash[25]==172 && hash[26]==11 && hash[27]==55 && hash[28]==6 && hash[29]==171 && hash[30]==66 && hash[31]==208)
            {
                return true;
            }
            else if(hash[0]==23 && hash[1]==91 && hash[2]==89 && hash[3]==145 && hash[4]==44 && hash[5]==254 && hash[6]==149 && hash[7]==164 && hash[8]==11 && hash[9]==200 && hash[10]==13 && hash[11]==206 && hash[12]==91 && hash[13]==109 && hash[14]==255 && hash[15]==32 && hash[16]==1 && hash[17]==250 && hash[18]==0 && hash[19]==217 && hash[20]==82 && hash[21]==72 && hash[22]==143 && hash[23]==5 && hash[24]==164 && hash[25]==168 && hash[26]==116 && hash[27]==186 && hash[28]==65 && hash[29]==69 && hash[30]==94 && hash[31]==209)
            {
                return true;
            }
            else if(hash[0]==230)
            {
                if(hash[1]==118 && hash[2]==97 && hash[3]==160 && hash[4]==174 && hash[5]==190 && hash[6]==54 && hash[7]==238 && hash[8]==103 && hash[9]==183 && hash[10]==188 && hash[11]==215 && hash[12]==213 && hash[13]==32 && hash[14]==189 && hash[15]==142 && hash[16]==73 && hash[17]==87 && hash[18]==246 && hash[19]==187 && hash[20]==22 && hash[21]==214 && hash[22]==196 && hash[23]==115 && hash[24]==40 && hash[25]==130 && hash[26]==112 && hash[27]==95 && hash[28]==62 && hash[29]==129 && hash[30]==202 && hash[31]==211)
                {
                    return true;
                }
                else if(hash[1]==144 && hash[2]==218 && hash[3]==47 && hash[4]==54 && hash[5]==46 && hash[6]==85 && hash[7]==221 && hash[8]==233 && hash[9]==100 && hash[10]==1 && hash[11]==143 && hash[12]==30 && hash[13]==8 && hash[14]==18 && hash[15]==157 && hash[16]==135 && hash[17]==232 && hash[18]==14 && hash[19]==231 && hash[20]==237 && hash[21]==88 && hash[22]==133 && hash[23]==106 && hash[24]==35 && hash[25]==231 && hash[26]==206 && hash[27]==153 && hash[28]==72 && hash[29]==204 && hash[30]==155 && hash[31]==2)
                {
                    return true;
                }
            }
            else if(hash[0]==231 && hash[1]==124 && hash[2]==0 && hash[3]==176 && hash[4]==221 && hash[5]==125 && hash[6]==133 && hash[7]==119 && hash[8]==222 && hash[9]==48 && hash[10]==251 && hash[11]==48 && hash[12]==165 && hash[13]==21 && hash[14]==138 && hash[15]==188 && hash[16]==71 && hash[17]==171 && hash[18]==105 && hash[19]==196 && hash[20]==81 && hash[21]==124 && hash[22]==41 && hash[23]==54 && hash[24]==39 && hash[25]==103 && hash[26]==164 && hash[27]==113 && hash[28]==145 && hash[29]==15 && hash[30]==208 && hash[31]==52)
            {
                return true;
            }
            else if(hash[0]==236 && hash[1]==110 && hash[2]==213 && hash[3]==197 && hash[4]==254 && hash[5]==9 && hash[6]==111 && hash[7]==77 && hash[8]==65 && hash[9]==55 && hash[10]==125 && hash[11]==42 && hash[12]==117 && hash[13]==60 && hash[14]==158 && hash[15]==189 && hash[16]==75 && hash[17]==40 && hash[18]==237 && hash[19]==213 && hash[20]==129 && hash[21]==153 && hash[22]==117 && hash[23]==196 && hash[24]==80 && hash[25]==58 && hash[26]==146 && hash[27]==4 && hash[28]==125 && hash[29]==94 && hash[30]==172 && hash[31]==233)
            {
                return true;
            }
            else if(hash[0]==238 && hash[1]==54 && hash[2]==42 && hash[3]==88 && hash[4]==224 && hash[5]==186 && hash[6]==167 && hash[7]==180 && hash[8]==227 && hash[9]==144 && hash[10]==190 && hash[11]==181 && hash[12]==200 && hash[13]==47 && hash[14]==47 && hash[15]==60 && hash[16]==44 && hash[17]==208 && hash[18]==124 && hash[19]==73 && hash[20]==71 && hash[21]==199 && hash[22]==88 && hash[23]==52 && hash[24]==119 && hash[25]==12 && hash[26]==250 && hash[27]==95 && hash[28]==19 && hash[29]==217 && hash[30]==71 && hash[31]==10)
            {
                return true;
            }
            else if(hash[0]==24 && hash[1]==188 && hash[2]==209 && hash[3]==145 && hash[4]==5 && hash[5]==148 && hash[6]==200 && hash[7]==104 && hash[8]==65 && hash[9]==106 && hash[10]==202 && hash[11]==67 && hash[12]==50 && hash[13]==92 && hash[14]==210 && hash[15]==222 && hash[16]==246 && hash[17]==2 && hash[18]==105 && hash[19]==225 && hash[20]==133 && hash[21]==119 && hash[22]==237 && hash[23]==174 && hash[24]==241 && hash[25]==52 && hash[26]==38 && hash[27]==198 && hash[28]==87 && hash[29]==57 && hash[30]==3 && hash[31]==8)
            {
                return true;
            }
            else if(hash[0]==246)
            {
                if(hash[1]==118 && hash[2]==21 && hash[3]==68 && hash[4]==95 && hash[5]==12 && hash[6]==227 && hash[7]==217 && hash[8]==130 && hash[9]==42 && hash[10]==223 && hash[11]==157 && hash[12]==54 && hash[13]==166 && hash[14]==107 && hash[15]==142 && hash[16]==227 && hash[17]==171 && hash[18]==168 && hash[19]==52 && hash[20]==228 && hash[21]==108 && hash[22]==142 && hash[23]==26 && hash[24]==114 && hash[25]==81 && hash[26]==160 && hash[27]==249 && hash[28]==132 && hash[29]==185 && hash[30]==13 && hash[31]==97)
                {
                    return true;
                }
                else if(hash[1]==215 && hash[2]==204 && hash[3]==251 && hash[4]==17 && hash[5]==185 && hash[6]==229 && hash[7]==120 && hash[8]==16 && hash[9]==251 && hash[10]==29 && hash[11]==81 && hash[12]==1 && hash[13]==131 && hash[14]==187 && hash[15]==159 && hash[16]==249 && hash[17]==140 && hash[18]==171 && hash[19]==103 && hash[20]==39 && hash[21]==6 && hash[22]==11 && hash[23]==18 && hash[24]==129 && hash[25]==153 && hash[26]==250 && hash[27]==18 && hash[28]==213 && hash[29]==42 && hash[30]==175 && hash[31]==241)
                {
                    return true;
                }
            }
            else if(hash[0]==248 && hash[1]==34 && hash[2]==48 && hash[3]==66 && hash[4]==72 && hash[5]==151 && hash[6]==40 && hash[7]==100 && hash[8]==117 && hash[9]==198 && hash[10]==63 && hash[11]==162 && hash[12]==161 && hash[13]==172 && hash[14]==219 && hash[15]==219 && hash[16]==227 && hash[17]==244 && hash[18]==190 && hash[19]==182 && hash[20]==66 && hash[21]==237 && hash[22]==225 && hash[23]==21 && hash[24]==12 && hash[25]==56 && hash[26]==111 && hash[27]==140 && hash[28]==64 && hash[29]==73 && hash[30]==21 && hash[31]==2)
            {
                return true;
            }
            else if(hash[0]==250 && hash[1]==34 && hash[2]==153 && hash[3]==171 && hash[4]==216 && hash[5]==48 && hash[6]==56 && hash[7]==4 && hash[8]==6 && hash[9]==217 && hash[10]==235 && hash[11]==119 && hash[12]==28 && hash[13]==15 && hash[14]==74 && hash[15]==196 && hash[16]==101 && hash[17]==143 && hash[18]==255 && hash[19]==127 && hash[20]==19 && hash[21]==194 && hash[22]==88 && hash[23]==13 && hash[24]==185 && hash[25]==221 && hash[26]==0 && hash[27]==81 && hash[28]==112 && hash[29]==163 && hash[30]==56 && hash[31]==182)
            {
                return true;
            }
            else if(hash[0]==253)
            {
                if(hash[1]==212 && hash[2]==22 && hash[3]==223 && hash[4]==193 && hash[5]==203 && hash[6]==159 && hash[7]==248 && hash[8]==198 && hash[9]==162 && hash[10]==248 && hash[11]==23 && hash[12]==204 && hash[13]==229 && hash[14]==143 && hash[15]==26 && hash[16]==204 && hash[17]==145 && hash[18]==180 && hash[19]==111 && hash[20]==197 && hash[21]==214 && hash[22]==162 && hash[23]==116 && hash[24]==97 && hash[25]==153 && hash[26]==225 && hash[27]==227 && hash[28]==72 && hash[29]==32 && hash[30]==125 && hash[31]==152)
                {
                    return true;
                }
                else if(hash[1]==255 && hash[2]==12 && hash[3]==0 && hash[4]==57 && hash[5]==182 && hash[6]==162 && hash[7]==219 && hash[8]==44 && hash[9]==148 && hash[10]==146 && hash[11]==35 && hash[12]==182 && hash[13]==153 && hash[14]==177 && hash[15]==49 && hash[16]==255 && hash[17]==90 && hash[18]==136 && hash[19]==186 && hash[20]==46 && hash[21]==242 && hash[22]==97 && hash[23]==211 && hash[24]==221 && hash[25]==237 && hash[26]==128 && hash[27]==21 && hash[28]==251 && hash[29]==74 && hash[30]==147 && hash[31]==5)
                {
                    return true;
                }
            }
            else if(hash[0]==3 && hash[1]==198 && hash[2]==119 && hash[3]==146 && hash[4]==120 && hash[5]==212 && hash[6]==89 && hash[7]==238 && hash[8]==22 && hash[9]==143 && hash[10]==13 && hash[11]==6 && hash[12]==165 && hash[13]==93 && hash[14]==223 && hash[15]==141 && hash[16]==243 && hash[17]==138 && hash[18]==128 && hash[19]==229 && hash[20]==236 && hash[21]==79 && hash[22]==89 && hash[23]==166 && hash[24]==208 && hash[25]==88 && hash[26]==240 && hash[27]==239 && hash[28]==155 && hash[29]==46 && hash[30]==160 && hash[31]==125)
            {
                return true;
            }
            else if(hash[0]==32 && hash[1]==162 && hash[2]==23 && hash[3]==99 && hash[4]==113 && hash[5]==31 && hash[6]==16 && hash[7]==120 && hash[8]==10 && hash[9]==165 && hash[10]==118 && hash[11]==66 && hash[12]==75 && hash[13]==39 && hash[14]==34 && hash[15]==167 && hash[16]==173 && hash[17]==129 && hash[18]==57 && hash[19]==136 && hash[20]==139 && hash[21]==111 && hash[22]==25 && hash[23]==156 && hash[24]==46 && hash[25]==165 && hash[26]==92 && hash[27]==29 && hash[28]==143 && hash[29]==80 && hash[30]==103 && hash[31]==206)
            {
                return true;
            }
            else if(hash[0]==33 && hash[1]==65 && hash[2]==235 && hash[3]==225 && hash[4]==247 && hash[5]==75 && hash[6]==189 && hash[7]==106 && hash[8]==229 && hash[9]==186 && hash[10]==124 && hash[11]==244 && hash[12]==175 && hash[13]==47 && hash[14]==20 && hash[15]==29 && hash[16]==251 && hash[17]==52 && hash[18]==127 && hash[19]==154 && hash[20]==142 && hash[21]==160 && hash[22]==146 && hash[23]==63 && hash[24]==233 && hash[25]==234 && hash[26]==90 && hash[27]==174 && hash[28]==127 && hash[29]==71 && hash[30]==169 && hash[31]==190)
            {
                return true;
            }
            else if(hash[0]==34)
            {
                if(hash[1]==19 && hash[2]==211 && hash[3]==28 && hash[4]==60 && hash[5]==201 && hash[6]==85 && hash[7]==121 && hash[8]==38 && hash[9]==139 && hash[10]==183 && hash[11]==101 && hash[12]==93 && hash[13]==126 && hash[14]==90 && hash[15]==17 && hash[16]==23 && hash[17]==218 && hash[18]==200 && hash[19]==226 && hash[20]==55 && hash[21]==196 && hash[22]==109 && hash[23]==81 && hash[24]==75 && hash[25]==31 && hash[26]==89 && hash[27]==54 && hash[28]==117 && hash[29]==204 && hash[30]==169 && hash[31]==217)
                {
                    return true;
                }
                else if(hash[1]==56 && hash[2]==15 && hash[3]==51 && hash[4]==89 && hash[5]==100 && hash[6]==52 && hash[7]==148 && hash[8]==223 && hash[9]==96 && hash[10]==112 && hash[11]==146 && hash[12]==179 && hash[13]==137 && hash[14]==37 && hash[15]==234 && hash[16]==84 && hash[17]==246 && hash[18]==19 && hash[19]==200 && hash[20]==45 && hash[21]==238 && hash[22]==54 && hash[23]==76 && hash[24]==231 && hash[25]==29 && hash[26]==137 && hash[27]==140 && hash[28]==46 && hash[29]==66 && hash[30]==30 && hash[31]==126)
                {
                    return true;
                }
            }
            else if(hash[0]==36 && hash[1]==106 && hash[2]==42 && hash[3]==101 && hash[4]==210 && hash[5]==248 && hash[6]==42 && hash[7]==74 && hash[8]==155 && hash[9]==246 && hash[10]==5 && hash[11]==204 && hash[12]==118 && hash[13]==127 && hash[14]==231 && hash[15]==29 && hash[16]==39 && hash[17]==231 && hash[18]==176 && hash[19]==79 && hash[20]==62 && hash[21]==112 && hash[22]==36 && hash[23]==94 && hash[24]==146 && hash[25]==114 && hash[26]==191 && hash[27]==184 && hash[28]==42 && hash[29]==212 && hash[30]==133 && hash[31]==239)
            {
                return true;
            }
            else if(hash[0]==37 && hash[1]==160 && hash[2]==61 && hash[3]==33 && hash[4]==196 && hash[5]==130 && hash[6]==127 && hash[7]==64 && hash[8]==88 && hash[9]==68 && hash[10]==197 && hash[11]==0 && hash[12]==19 && hash[13]==198 && hash[14]==66 && hash[15]==89 && hash[16]==67 && hash[17]==232 && hash[18]==64 && hash[19]==185 && hash[20]==242 && hash[21]==29 && hash[22]==177 && hash[23]==255 && hash[24]==194 && hash[25]==44 && hash[26]==200 && hash[27]==24 && hash[28]==243 && hash[29]==197 && hash[30]==82 && hash[31]==112)
            {
                return true;
            }
            else if(hash[0]==39 && hash[1]==98 && hash[2]==159 && hash[3]==112 && hash[4]==162 && hash[5]==183 && hash[6]==212 && hash[7]==144 && hash[8]==132 && hash[9]==130 && hash[10]==148 && hash[11]==50 && hash[12]==70 && hash[13]==142 && hash[14]==85 && hash[15]==89 && hash[16]==19 && hash[17]==152 && hash[18]==213 && hash[19]==111 && hash[20]==160 && hash[21]==8 && hash[22]==126 && hash[23]==184 && hash[24]==240 && hash[25]==248 && hash[26]==41 && hash[27]==223 && hash[28]==106 && hash[29]==30 && hash[30]==221 && hash[31]==211)
            {
                return true;
            }
            else if(hash[0]==41)
            {
                if(hash[1]==158 && hash[2]==140 && hash[3]==234 && hash[4]==76 && hash[5]==2 && hash[6]==124 && hash[7]==55 && hash[8]==83 && hash[9]==44 && hash[10]==70 && hash[11]==76 && hash[12]==93 && hash[13]==187 && hash[14]==252 && hash[15]==70 && hash[16]==95 && hash[17]==23 && hash[18]==134 && hash[19]==21 && hash[20]==218 && hash[21]==159 && hash[22]==69 && hash[23]==91 && hash[24]==51 && hash[25]==135 && hash[26]==68 && hash[27]==33 && hash[28]==203 && hash[29]==154 && hash[30]==181 && hash[31]==149)
                {
                    return true;
                }
                else if(hash[1]==84 && hash[2]==151 && hash[3]==213 && hash[4]==254 && hash[5]==162 && hash[6]==63 && hash[7]==69 && hash[8]==113 && hash[9]==239 && hash[10]==65 && hash[11]==116 && hash[12]==147 && hash[13]==225 && hash[14]==34 && hash[15]==162 && hash[16]==156 && hash[17]==178 && hash[18]==39 && hash[19]==210 && hash[20]==9 && hash[21]==218 && hash[22]==229 && hash[23]==182 && hash[24]==162 && hash[25]==143 && hash[26]==162 && hash[27]==253 && hash[28]==101 && hash[29]==77 && hash[30]==49 && hash[31]==147)
                {
                    return true;
                }
            }
            else if(hash[0]==44 && hash[1]==101 && hash[2]==162 && hash[3]==134 && hash[4]==150 && hash[5]==81 && hash[6]==233 && hash[7]==87 && hash[8]==214 && hash[9]==60 && hash[10]==253 && hash[11]==17 && hash[12]==24 && hash[13]==126 && hash[14]==83 && hash[15]==31 && hash[16]==138 && hash[17]==235 && hash[18]==138 && hash[19]==138 && hash[20]==120 && hash[21]==26 && hash[22]==204 && hash[23]==23 && hash[24]==247 && hash[25]==94 && hash[26]==136 && hash[27]==211 && hash[28]==153 && hash[29]==30 && hash[30]==246 && hash[31]==38)
            {
                return true;
            }
            else if(hash[0]==46)
            {
                if(hash[1]==207 && hash[2]==95 && hash[3]==134 && hash[4]==98 && hash[5]==225 && hash[6]==166 && hash[7]==209 && hash[8]==106 && hash[9]==140 && hash[10]==5 && hash[11]==124 && hash[12]==12 && hash[13]==76 && hash[14]==7 && hash[15]==177 && hash[16]==219 && hash[17]==243 && hash[18]==21 && hash[19]==235 && hash[20]==38 && hash[21]==254 && hash[22]==231 && hash[23]==104 && hash[24]==118 && hash[25]==102 && hash[26]==245 && hash[27]==222 && hash[28]==210 && hash[29]==7 && hash[30]==40 && hash[31]==220)
                {
                    return true;
                }
                else if(hash[1]==34 && hash[2]==20 && hash[3]==219 && hash[4]==236 && hash[5]==236 && hash[6]==73 && hash[7]==182 && hash[8]==252 && hash[9]==224 && hash[10]==243 && hash[11]==97 && hash[12]==98 && hash[13]==120 && hash[14]==250 && hash[15]==140 && hash[16]==185 && hash[17]==182 && hash[18]==234 && hash[19]==158 && hash[20]==93 && hash[21]==145 && hash[22]==137 && hash[23]==119 && hash[24]==246 && hash[25]==75 && hash[26]==112 && hash[27]==144 && hash[28]==206 && hash[29]==201 && hash[30]==165 && hash[31]==64)
                {
                    return true;
                }
            }
            else if(hash[0]==49 && hash[1]==138 && hash[2]==217 && hash[3]==143 && hash[4]==49 && hash[5]==74 && hash[6]==166 && hash[7]==237 && hash[8]==52 && hash[9]==170 && hash[10]==125 && hash[11]==216 && hash[12]==224 && hash[13]==55 && hash[14]==140 && hash[15]==104 && hash[16]==66 && hash[17]==208 && hash[18]==150 && hash[19]==87 && hash[20]==7 && hash[21]==107 && hash[22]==0 && hash[23]==130 && hash[24]==105 && hash[25]==183 && hash[26]==30 && hash[27]==198 && hash[28]==15 && hash[29]==93 && hash[30]==18 && hash[31]==212)
            {
                return true;
            }
            else if(hash[0]==56 && hash[1]==123 && hash[2]==179 && hash[3]==238 && hash[4]==40 && hash[5]==110 && hash[6]==14 && hash[7]==255 && hash[8]==198 && hash[9]==219 && hash[10]==30 && hash[11]==28 && hash[12]==89 && hash[13]==177 && hash[14]==57 && hash[15]==253 && hash[16]==101 && hash[17]==169 && hash[18]==91 && hash[19]==150 && hash[20]==207 && hash[21]==138 && hash[22]==148 && hash[23]==167 && hash[24]==169 && hash[25]==68 && hash[26]==69 && hash[27]==43 && hash[28]==172 && hash[29]==232 && hash[30]==61 && hash[31]==35)
            {
                return true;
            }
            else if(hash[0]==57 && hash[1]==148 && hash[2]==65 && hash[3]==167 && hash[4]==74 && hash[5]==102 && hash[6]==165 && hash[7]==175 && hash[8]==177 && hash[9]==183 && hash[10]==71 && hash[11]==28 && hash[12]==246 && hash[13]==126 && hash[14]==108 && hash[15]==49 && hash[16]==115 && hash[17]==208 && hash[18]==185 && hash[19]==229 && hash[20]==30 && hash[21]==40 && hash[22]==47 && hash[23]==234 && hash[24]==120 && hash[25]==182 && hash[26]==243 && hash[27]==132 && hash[28]==185 && hash[29]==147 && hash[30]==168 && hash[31]==43)
            {
                return true;
            }
            else if(hash[0]==58 && hash[1]==189 && hash[2]==129 && hash[3]==198 && hash[4]==110 && hash[5]==210 && hash[6]==233 && hash[7]==47 && hash[8]==104 && hash[9]==239 && hash[10]==217 && hash[11]==29 && hash[12]==193 && hash[13]==136 && hash[14]==92 && hash[15]==170 && hash[16]==22 && hash[17]==34 && hash[18]==244 && hash[19]==1 && hash[20]==39 && hash[21]==114 && hash[22]==215 && hash[23]==49 && hash[24]==244 && hash[25]==200 && hash[26]==201 && hash[27]==207 && hash[28]==233 && hash[29]==61 && hash[30]==131 && hash[31]==117)
            {
                return true;
            }
            else if(hash[0]==59 && hash[1]==31 && hash[2]==74 && hash[3]==55 && hash[4]==109 && hash[5]==105 && hash[6]==111 && hash[7]==97 && hash[8]==191 && hash[9]==29 && hash[10]==124 && hash[11]==146 && hash[12]==11 && hash[13]==91 && hash[14]==236 && hash[15]==76 && hash[16]==191 && hash[17]==183 && hash[18]==93 && hash[19]==75 && hash[20]==62 && hash[21]==28 && hash[22]==46 && hash[23]==251 && hash[24]==149 && hash[25]==221 && hash[26]==239 && hash[27]==13 && hash[28]==85 && hash[29]==204 && hash[30]==165 && hash[31]==139)
            {
                return true;
            }
            else if(hash[0]==6 && hash[1]==108 && hash[2]==121 && hash[3]==72 && hash[4]==78 && hash[5]==58 && hash[6]==86 && hash[7]==26 && hash[8]==155 && hash[9]==251 && hash[10]==237 && hash[11]==46 && hash[12]==55 && hash[13]==232 && hash[14]==4 && hash[15]==144 && hash[16]==177 && hash[17]==45 && hash[18]==209 && hash[19]==217 && hash[20]==102 && hash[21]==104 && hash[22]==42 && hash[23]==59 && hash[24]==24 && hash[25]==79 && hash[26]==227 && hash[27]==246 && hash[28]==220 && hash[29]==79 && hash[30]==25 && hash[31]==124)
            {
                return true;
            }
            else if(hash[0]==62 && hash[1]==10 && hash[2]==35 && hash[3]==215 && hash[4]==148 && hash[5]==185 && hash[6]==5 && hash[7]==33 && hash[8]==113 && hash[9]==217 && hash[10]==138 && hash[11]==227 && hash[12]==194 && hash[13]==252 && hash[14]==170 && hash[15]==205 && hash[16]==108 && hash[17]==180 && hash[18]==21 && hash[19]==157 && hash[20]==219 && hash[21]==129 && hash[22]==94 && hash[23]==31 && hash[24]==107 && hash[25]==239 && hash[26]==195 && hash[27]==120 && hash[28]==211 && hash[29]==22 && hash[30]==80 && hash[31]==227)
            {
                return true;
            }
            else if(hash[0]==64 && hash[1]==50 && hash[2]==40 && hash[3]==208 && hash[4]==108 && hash[5]==175 && hash[6]==92 && hash[7]==28 && hash[8]==89 && hash[9]==5 && hash[10]==252 && hash[11]==245 && hash[12]==101 && hash[13]==193 && hash[14]==176 && hash[15]==103 && hash[16]==98 && hash[17]==207 && hash[18]==78 && hash[19]==233 && hash[20]==200 && hash[21]==190 && hash[22]==190 && hash[23]==107 && hash[24]==172 && hash[25]==136 && hash[26]==39 && hash[27]==171 && hash[28]==128 && hash[29]==137 && hash[30]==110 && hash[31]==117)
            {
                return true;
            }
            else if(hash[0]==65 && hash[1]==220 && hash[2]==160 && hash[3]==102 && hash[4]==206 && hash[5]==78 && hash[6]==161 && hash[7]==255 && hash[8]==8 && hash[9]==67 && hash[10]==177 && hash[11]==246 && hash[12]==21 && hash[13]==55 && hash[14]==72 && hash[15]==192 && hash[16]==8 && hash[17]==202 && hash[18]==91 && hash[19]==190 && hash[20]==190 && hash[21]==48 && hash[22]==189 && hash[23]==190 && hash[24]==45 && hash[25]==138 && hash[26]==15 && hash[27]==8 && hash[28]==73 && hash[29]==120 && hash[30]==212 && hash[31]==1)
            {
                return true;
            }
            else if(hash[0]==70 && hash[1]==37 && hash[2]==255 && hash[3]==92 && hash[4]==188 && hash[5]==73 && hash[6]==250 && hash[7]==89 && hash[8]==9 && hash[9]==117 && hash[10]==28 && hash[11]==87 && hash[12]==49 && hash[13]==134 && hash[14]==235 && hash[15]==143 && hash[16]==159 && hash[17]==17 && hash[18]==60 && hash[19]==51 && hash[20]==180 && hash[21]==191 && hash[22]==136 && hash[23]==210 && hash[24]==247 && hash[25]==143 && hash[26]==61 && hash[27]==3 && hash[28]==72 && hash[29]==90 && hash[30]==222 && hash[31]==189)
            {
                return true;
            }
            else if(hash[0]==71 && hash[1]==177 && hash[2]==111 && hash[3]==204 && hash[4]==220 && hash[5]==247 && hash[6]==195 && hash[7]==55 && hash[8]==212 && hash[9]==161 && hash[10]==233 && hash[11]==154 && hash[12]==231 && hash[13]==196 && hash[14]==122 && hash[15]==43 && hash[16]==121 && hash[17]==142 && hash[18]==168 && hash[19]==62 && hash[20]==224 && hash[21]==188 && hash[22]==102 && hash[23]==190 && hash[24]==100 && hash[25]==115 && hash[26]==188 && hash[27]==250 && hash[28]==168 && hash[29]==168 && hash[30]==94 && hash[31]==54)
            {
                return true;
            }
            else if(hash[0]==78 && hash[1]==83 && hash[2]==27 && hash[3]==170 && hash[4]==245 && hash[5]==2 && hash[6]==250 && hash[7]==60 && hash[8]==247 && hash[9]==182 && hash[10]==25 && hash[11]==4 && hash[12]==127 && hash[13]==161 && hash[14]==183 && hash[15]==177 && hash[16]==209 && hash[17]==70 && hash[18]==43 && hash[19]==224 && hash[20]==209 && hash[21]==173 && hash[22]==25 && hash[23]==250 && hash[24]==118 && hash[25]==95 && hash[26]==182 && hash[27]==163 && hash[28]==63 && hash[29]==234 && hash[30]==180 && hash[31]==244)
            {
                return true;
            }
            else if(hash[0]==79 && hash[1]==246 && hash[2]==48 && hash[3]==75 && hash[4]==107 && hash[5]==49 && hash[6]==155 && hash[7]==235 && hash[8]==125 && hash[9]==27 && hash[10]==106 && hash[11]==208 && hash[12]==197 && hash[13]==251 && hash[14]==148 && hash[15]==203 && hash[16]==89 && hash[17]==182 && hash[18]==44 && hash[19]==152 && hash[20]==151 && hash[21]==243 && hash[22]==66 && hash[23]==44 && hash[24]==36 && hash[25]==187 && hash[26]==250 && hash[27]==213 && hash[28]==81 && hash[29]==154 && hash[30]==165 && hash[31]==226)
            {
                return true;
            }
            else if(hash[0]==83 && hash[1]==158 && hash[2]==57 && hash[3]==85 && hash[4]==56 && hash[5]==130 && hash[6]==241 && hash[7]==99 && hash[8]==144 && hash[9]==117 && hash[10]==24 && hash[11]==184 && hash[12]==77 && hash[13]==19 && hash[14]==119 && hash[15]==201 && hash[16]==18 && hash[17]==83 && hash[18]==164 && hash[19]==150 && hash[20]==214 && hash[21]==176 && hash[22]==188 && hash[23]==168 && hash[24]==176 && hash[25]==252 && hash[26]==202 && hash[27]==105 && hash[28]==236 && hash[29]==51 && hash[30]==110 && hash[31]==155)
            {
                return true;
            }
            else if(hash[0]==84 && hash[1]==133 && hash[2]==201 && hash[3]==164 && hash[4]==20 && hash[5]==48 && hash[6]==103 && hash[7]==30 && hash[8]==194 && hash[9]==159 && hash[10]==116 && hash[11]==32 && hash[12]==178 && hash[13]==45 && hash[14]==118 && hash[15]==175 && hash[16]==168 && hash[17]==201 && hash[18]==196 && hash[19]==137 && hash[20]==38 && hash[21]==213 && hash[22]==168 && hash[23]==108 && hash[24]==201 && hash[25]==199 && hash[26]==159 && hash[27]==187 && hash[28]==88 && hash[29]==10 && hash[30]==105 && hash[31]==48)
            {
                return true;
            }
            else if(hash[0]==86 && hash[1]==109 && hash[2]==186 && hash[3]==150 && hash[4]==26 && hash[5]==118 && hash[6]==242 && hash[7]==251 && hash[8]==110 && hash[9]==56 && hash[10]==128 && hash[11]==140 && hash[12]==237 && hash[13]==232 && hash[14]==46 && hash[15]==225 && hash[16]==238 && hash[17]==9 && hash[18]==102 && hash[19]==167 && hash[20]==119 && hash[21]==222 && hash[22]==159 && hash[23]==134 && hash[24]==235 && hash[25]==173 && hash[26]==17 && hash[27]==142 && hash[28]==82 && hash[29]==21 && hash[30]==175 && hash[31]==235)
            {
                return true;
            }
            else if(hash[0]==87 && hash[1]==201 && hash[2]==102 && hash[3]==86 && hash[4]==232 && hash[5]==124 && hash[6]==29 && hash[7]==181 && hash[8]==139 && hash[9]==13 && hash[10]==86 && hash[11]==30 && hash[12]==142 && hash[13]==201 && hash[14]==200 && hash[15]==129 && hash[16]==201 && hash[17]==145 && hash[18]==229 && hash[19]==37 && hash[20]==163 && hash[21]==129 && hash[22]==232 && hash[23]==158 && hash[24]==134 && hash[25]==75 && hash[26]==180 && hash[27]==234 && hash[28]==236 && hash[29]==93 && hash[30]==130 && hash[31]==231)
            {
                return true;
            }
            else if(hash[0]==92 && hash[1]==62 && hash[2]==27 && hash[3]==155 && hash[4]==12 && hash[5]==188 && hash[6]==177 && hash[7]==69 && hash[8]==1 && hash[9]==201 && hash[10]==225 && hash[11]==120 && hash[12]==248 && hash[13]==106 && hash[14]==34 && hash[15]==80 && hash[16]==130 && hash[17]==126 && hash[18]==36 && hash[19]==208 && hash[20]==205 && hash[21]==107 && hash[22]==24 && hash[23]==90 && hash[24]==5 && hash[25]==98 && hash[26]==114 && hash[27]==9 && hash[28]==214 && hash[29]==168 && hash[30]==83 && hash[31]==98)
            {
                return true;
            }
            else if(hash[0]==97)
            {
                if(hash[1]==170 && hash[2]==164 && hash[3]==10 && hash[4]==186 && hash[5]==110 && hash[6]==84 && hash[7]==73 && hash[8]==196 && hash[9]==86 && hash[10]==133 && hash[11]==210 && hash[12]==182 && hash[13]==33 && hash[14]==199 && hash[15]==156 && hash[16]==197 && hash[17]==121 && hash[18]==44 && hash[19]==184 && hash[20]==139 && hash[21]==109 && hash[22]==193 && hash[23]==8 && hash[24]==127 && hash[25]==246 && hash[26]==102 && hash[27]==155 && hash[28]==165 && hash[29]==120 && hash[30]==51 && hash[31]==229)
                {
                    return true;
                }
                else if(hash[1]==48 && hash[2]==207 && hash[3]==215 && hash[4]==142 && hash[5]==50 && hash[6]==86 && hash[7]==11 && hash[8]==232 && hash[9]==44 && hash[10]==185 && hash[11]==242 && hash[12]==83 && hash[13]==47 && hash[14]==144 && hash[15]==55 && hash[16]==142 && hash[17]==82 && hash[18]==172 && hash[19]==190 && hash[20]==43 && hash[21]==81 && hash[22]==233 && hash[23]==34 && hash[24]==173 && hash[25]==107 && hash[26]==69 && hash[27]==203 && hash[28]==118 && hash[29]==187 && hash[30]==88 && hash[31]==52)
                {
                    return true;
                }
            }

            return false;
        }
    }
}