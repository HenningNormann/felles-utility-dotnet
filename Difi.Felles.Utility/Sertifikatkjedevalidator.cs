﻿using System.Security.Cryptography.X509Certificates;

namespace Difi.Felles.Utility
{
    public class Sertifikatkjedevalidator
    {
        public X509Certificate2Collection SertifikatLager { get; set; }

        public Sertifikatkjedevalidator(X509Certificate2Collection sertifikatLager)
        {
            SertifikatLager = sertifikatLager;
        }

        public bool ErGyldigResponssertifikat(X509Certificate2 sertifikat)
        {
            X509ChainStatus[] kjedestatus;
            return ErGyldigResponssertifikat(sertifikat, out kjedestatus);
        }

        public bool ErGyldigResponssertifikat(X509Certificate2 sertifikat, out X509ChainStatus[] kjedestatus)
        {
            var chain = new X509Chain()
            {
                ChainPolicy = ChainPolicy()
            };

            var erGyldigResponssertifikat = chain.Build(sertifikat);
            if (!erGyldigResponssertifikat)
            {
                erGyldigResponssertifikat = ErGyldigResponssertifikatHvisKunUntrustedRoot(chain);
            }

            kjedestatus = chain.ChainStatus;
            return erGyldigResponssertifikat;
        }

        public X509ChainPolicy ChainPolicy()
        {
            var policy = new X509ChainPolicy()
            {
                RevocationMode = X509RevocationMode.NoCheck

            };
            policy.ExtraStore.AddRange(SertifikatLager);

            return policy;
        }

        private static bool ErGyldigResponssertifikatHvisKunUntrustedRoot(X509Chain chain)
        {
            var erGyldigResponssertifikat = false;
            const int forventetKjedelengde = 3;
            const int forventetAntallKjedeStatuselementer = 1;

            var kjedeElementer = chain.ChainElements;
            var erKjedeMedTreSertifikater = kjedeElementer.Count == forventetKjedelengde;
            var erUntrustedRoot = chain.ChainStatus.Length == forventetAntallKjedeStatuselementer && chain.ChainStatus[0].Status == X509ChainStatusFlags.UntrustedRoot;
            if (erKjedeMedTreSertifikater && erUntrustedRoot)
            {
                erGyldigResponssertifikat = true;
            }

            return erGyldigResponssertifikat;
        }
    }
}