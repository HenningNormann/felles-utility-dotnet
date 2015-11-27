﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;

namespace Difi.Felles.Utility.Utilities
{
    public static class SertifikatkjedeUtility
    {
        static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.Felles.Utility.Ressurser.sertifikater");

        public static X509Certificate2Collection FunksjoneltTestmiljøSertifikater()
        {
            var difiTestkjedesertifikater = new List<X509Certificate2>
            {
                new X509Certificate2(ResourceUtility.ReadAllBytes(true,"test", "Buypass_Class_3_Test4_CA_3.cer")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true,"test", "Buypass_Class_3_Test4_Root_CA.cer")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true,"test", "intermediate - commfides cpn enterprise-norwegian sha256 ca - test2.crt")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true,"test", "root - cpn root sha256 ca - test.crt"))
            };
            return new X509Certificate2Collection(difiTestkjedesertifikater.ToArray());
        }

        public static X509Certificate2Collection ProduksjonsSertifikater()
        {
            var difiProduksjonssertifikater = new List<X509Certificate2>
            {
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "produksjon", "BPClass3CA3.cer")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "produksjon", "BPClass3RootCA.cer")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "produksjon", "cpn enterprise sha256 class 3.crt")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "produksjon", "cpn rootca sha256 class 3.crt"))
            }
;
            return new X509Certificate2Collection(difiProduksjonssertifikater.ToArray());
        }
    }
}
