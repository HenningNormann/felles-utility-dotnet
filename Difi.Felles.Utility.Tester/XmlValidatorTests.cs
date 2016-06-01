﻿using System.IO;
using System.Xml;
using ApiClientShared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.Felles.Utility.Tests
{
    [TestClass]
    public class XmlValidatorTests
    {
        [TestMethod]
        public void ValidateMedKorrektXmlBurdeIkkeGiNoenWarningsOgReturnereTrue()
        {
            const string ugyldigPersonnummer = "12124536122";
            var testRequestMedUgyldigPersonnummer = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\"><soap:Header><wsse:Security xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\"><wsu:Timestamp wsu:Id=\"TS-008fc455-74f1-4276-a059-5a557ea8a319\"><wsu:Created>2016-06-01T12:35:20.967Z</wsu:Created><wsu:Expires>2016-06-01T13:05:20.967Z</wsu:Expires></wsu:Timestamp><wsse:BinarySecurityToken EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\" wsu:Id=\"X509-e068095a-3fa9-470d-92f1-e000ea273fa9\">MIIFOjCCBCKgAwIBAgIKGQqI22LuZ+0U6TANBgkqhkiG9w0BAQsFADBRMQswCQYDVQQGEwJOTzEdMBsGA1UECgwUQnV5cGFzcyBBUy05ODMxNjMzMjcxIzAhBgNVBAMMGkJ1eXBhc3MgQ2xhc3MgMyBUZXN0NCBDQSAzMB4XDTE0MDYxNjA4NTYyNloXDTE3MDYxNjIxNTkwMFowgaAxCzAJBgNVBAYTAk5PMSwwKgYDVQQKDCNESVJFS1RPUkFURVQgRk9SIEZPUlZBTFROSU5HIE9HIElLVDEhMB8GA1UECwwYU0RQIC0gbWVsZGluZ3N1dHZla3NsaW5nMSwwKgYDVQQDDCNESVJFS1RPUkFURVQgRk9SIEZPUlZBTFROSU5HIE9HIElLVDESMBAGA1UEBRMJOTkxODI1ODI3MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAx6IPA2KSAkSupen5fFM1LEnW6CRqSK20wjpBnXf414W03eWUvBlw97c6k5sl2tYdn4aVb6Z9GeDaz1bLKN3XwhFGPk9PnjSIhrFJNAPnWVEBDqGqfeMrEsYdOEgM2veBZDYkhVwipjr8AesmptTRAat61q+6hCJe8UZqjXb4Mg6YKSTAHfJdthAG06weBMgVouQkTkeIIawM+QPcKQ3Wao0gIZi17V0+8xzgDu1PXr90eJ/Xjsw9t0C8Ey/3N7n3j3hplsZkjOJMBNHzbeBG/doroC6uzVURiuEn9Bc9Nk224b+7lOBZ1FvNNrJVUu5Ty3xyMDseCV7z1QTwW7wcpwIDAQABo4IBwjCCAb4wCQYDVR0TBAIwADAfBgNVHSMEGDAWgBQ/rvV4C5KjcCA1X1r69ySgUgHwQTAdBgNVHQ4EFgQU6JguiqDjkgjEGRHhzkbeKeqyWQEwDgYDVR0PAQH/BAQDAgSwMBYGA1UdIAQPMA0wCwYJYIRCARoBAAMCMIG7BgNVHR8EgbMwgbAwN6A1oDOGMWh0dHA6Ly9jcmwudGVzdDQuYnV5cGFzcy5uby9jcmwvQlBDbGFzczNUNENBMy5jcmwwdaBzoHGGb2xkYXA6Ly9sZGFwLnRlc3Q0LmJ1eXBhc3Mubm8vZGM9QnV5cGFzcyxkYz1OTyxDTj1CdXlwYXNzJTIwQ2xhc3MlMjAzJTIwVGVzdDQlMjBDQSUyMDM/Y2VydGlmaWNhdGVSZXZvY2F0aW9uTGlzdDCBigYIKwYBBQUHAQEEfjB8MDsGCCsGAQUFBzABhi9odHRwOi8vb2NzcC50ZXN0NC5idXlwYXNzLm5vL29jc3AvQlBDbGFzczNUNENBMzA9BggrBgEFBQcwAoYxaHR0cDovL2NydC50ZXN0NC5idXlwYXNzLm5vL2NydC9CUENsYXNzM1Q0Q0EzLmNlcjANBgkqhkiG9w0BAQsFAAOCAQEAKOTM1zSdGHWUBKPzDPYCcci9cpbktd2WuBg028bRC0NwKSWUKeuUfWesTiu/P4UlYGe86qd/+z3MNpN89aGA8pr0E0WpI+NM+v+Cb0dQwxHASHtrkVo9CVx6V6/QSBqIUEMfNquDHzxB2/mXbv6GuO5eIl3OSVKg7Ffd/1wdE6zeMmHQO+zRpfj+OVEhNPb5cLa13Ah9+JrMkr1O7VUFbozLQgFPhuI8/5+u8U/6cDOOmcFV4f4IYUmhbcLiW5MQnvaJ8044+uInOQTNtSkKmZAo7Jnm4KUyhFXftJOStOHSlODOQcepVS7csszO5yWQRMTV8doEsaH5p/LBXYF56Q==</wsse:BinarySecurityToken><Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\"><SignedInfo><CanonicalizationMethod Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\" /><SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256\" /><Reference URI=\"#TS-008fc455-74f1-4276-a059-5a557ea8a319\"><DigestMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#sha1\" /><DigestValue>jPyIV0IZVmfX4/v7AHA6DrNp4f0=</DigestValue></Reference><Reference URI=\"#id-c49f5d4e-13b4-4ad1-b4ca-42ab885c361b\"><Transforms><Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\"><InclusiveNamespaces PrefixList=\"\" xmlns=\"http://www.w3.org/2001/10/xml-exc-c14n#\" /></Transform></Transforms><DigestMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#sha1\" /><DigestValue>w8pBYe0HPY4OOgZyAeP1RikWQVc=</DigestValue></Reference></SignedInfo><SignatureValue>F4A2OAAHLFhhPqZx/molFQf/TBqABjW88RrVriI/lPLVYtPT7EMkUa5eWZzMne082NLr+yZXb0Ce6uz21lHwOwPe7QovtggFTZw7AgBJkBlvW1CgKpCobM71fWy/vyE02Ekh4gkis2q5OwjWw3bQfEI3HD/EGduuz3yex40iU71twl+xl47wQC1VctpX7l2j5wZiz5GbHg57RWKF3VFRP9ErAUNRx7hmIXKwvv5gh2jrtgoapXzfVhi8RyVs/DATqLJKo/5WWSgUd5PLEiVl0F/UzCSnUKFHN+ET9hn/C3RkHBghhucf7fb1j7FaWMaL5vK3/llU8Fh+YgCodFCDAg==</SignatureValue><KeyInfo Id=\"KS-65ebcfc5-37e1-425a-ab23-b7ccc013894f\"><wsse:SecurityTokenReference wsu:Id=\"STR-4cf359f5-a0a8-4d48-b199-7d6bde22041a\"><Reference URI=\"#X509-e068095a-3fa9-470d-92f1-e000ea273fa9\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\" xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" /></wsse:SecurityTokenReference></KeyInfo></Signature></wsse:Security></soap:Header><soap:Body wsu:Id=\"id-c49f5d4e-13b4-4ad1-b4ca-42ab885c361b\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\"><ns:HentPersonerForespoersel xmlns:ns=\"http://kontaktinfo.difi.no/xsd/oppslagstjeneste/16-02\"><ns:informasjonsbehov>Sertifikat</ns:informasjonsbehov><ns:informasjonsbehov>Kontaktinfo</ns:informasjonsbehov><ns:informasjonsbehov>SikkerDigitalPost</ns:informasjonsbehov><ns:personidentifikator>{ugyldigPersonnummer}</ns:personidentifikator></ns:HentPersonerForespoersel></soap:Body></soap:Envelope>";
            
            XmlValidator validator = new XmlvalidatorImpl();
            var validateResult = validator.Validate(testRequestMedUgyldigPersonnummer);
            var warnings = validator.ValidationWarnings;

            Assert.IsNull(warnings);
            Assert.IsTrue(validateResult, "Burde være positivt resultat på valideringen.");
        }

        [TestMethod]
        public void ValidateMedUgyldigPersonnummerBurdeGiWarningOgReturnereFalse()
        {
            const string ugyldigPersonnummer = "121245361221";
            var testRequestMedUgyldigPersonnummer = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\"><soap:Header><wsse:Security xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\"><wsu:Timestamp wsu:Id=\"TS-008fc455-74f1-4276-a059-5a557ea8a319\"><wsu:Created>2016-06-01T12:35:20.967Z</wsu:Created><wsu:Expires>2016-06-01T13:05:20.967Z</wsu:Expires></wsu:Timestamp><wsse:BinarySecurityToken EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\" wsu:Id=\"X509-e068095a-3fa9-470d-92f1-e000ea273fa9\">MIIFOjCCBCKgAwIBAgIKGQqI22LuZ+0U6TANBgkqhkiG9w0BAQsFADBRMQswCQYDVQQGEwJOTzEdMBsGA1UECgwUQnV5cGFzcyBBUy05ODMxNjMzMjcxIzAhBgNVBAMMGkJ1eXBhc3MgQ2xhc3MgMyBUZXN0NCBDQSAzMB4XDTE0MDYxNjA4NTYyNloXDTE3MDYxNjIxNTkwMFowgaAxCzAJBgNVBAYTAk5PMSwwKgYDVQQKDCNESVJFS1RPUkFURVQgRk9SIEZPUlZBTFROSU5HIE9HIElLVDEhMB8GA1UECwwYU0RQIC0gbWVsZGluZ3N1dHZla3NsaW5nMSwwKgYDVQQDDCNESVJFS1RPUkFURVQgRk9SIEZPUlZBTFROSU5HIE9HIElLVDESMBAGA1UEBRMJOTkxODI1ODI3MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAx6IPA2KSAkSupen5fFM1LEnW6CRqSK20wjpBnXf414W03eWUvBlw97c6k5sl2tYdn4aVb6Z9GeDaz1bLKN3XwhFGPk9PnjSIhrFJNAPnWVEBDqGqfeMrEsYdOEgM2veBZDYkhVwipjr8AesmptTRAat61q+6hCJe8UZqjXb4Mg6YKSTAHfJdthAG06weBMgVouQkTkeIIawM+QPcKQ3Wao0gIZi17V0+8xzgDu1PXr90eJ/Xjsw9t0C8Ey/3N7n3j3hplsZkjOJMBNHzbeBG/doroC6uzVURiuEn9Bc9Nk224b+7lOBZ1FvNNrJVUu5Ty3xyMDseCV7z1QTwW7wcpwIDAQABo4IBwjCCAb4wCQYDVR0TBAIwADAfBgNVHSMEGDAWgBQ/rvV4C5KjcCA1X1r69ySgUgHwQTAdBgNVHQ4EFgQU6JguiqDjkgjEGRHhzkbeKeqyWQEwDgYDVR0PAQH/BAQDAgSwMBYGA1UdIAQPMA0wCwYJYIRCARoBAAMCMIG7BgNVHR8EgbMwgbAwN6A1oDOGMWh0dHA6Ly9jcmwudGVzdDQuYnV5cGFzcy5uby9jcmwvQlBDbGFzczNUNENBMy5jcmwwdaBzoHGGb2xkYXA6Ly9sZGFwLnRlc3Q0LmJ1eXBhc3Mubm8vZGM9QnV5cGFzcyxkYz1OTyxDTj1CdXlwYXNzJTIwQ2xhc3MlMjAzJTIwVGVzdDQlMjBDQSUyMDM/Y2VydGlmaWNhdGVSZXZvY2F0aW9uTGlzdDCBigYIKwYBBQUHAQEEfjB8MDsGCCsGAQUFBzABhi9odHRwOi8vb2NzcC50ZXN0NC5idXlwYXNzLm5vL29jc3AvQlBDbGFzczNUNENBMzA9BggrBgEFBQcwAoYxaHR0cDovL2NydC50ZXN0NC5idXlwYXNzLm5vL2NydC9CUENsYXNzM1Q0Q0EzLmNlcjANBgkqhkiG9w0BAQsFAAOCAQEAKOTM1zSdGHWUBKPzDPYCcci9cpbktd2WuBg028bRC0NwKSWUKeuUfWesTiu/P4UlYGe86qd/+z3MNpN89aGA8pr0E0WpI+NM+v+Cb0dQwxHASHtrkVo9CVx6V6/QSBqIUEMfNquDHzxB2/mXbv6GuO5eIl3OSVKg7Ffd/1wdE6zeMmHQO+zRpfj+OVEhNPb5cLa13Ah9+JrMkr1O7VUFbozLQgFPhuI8/5+u8U/6cDOOmcFV4f4IYUmhbcLiW5MQnvaJ8044+uInOQTNtSkKmZAo7Jnm4KUyhFXftJOStOHSlODOQcepVS7csszO5yWQRMTV8doEsaH5p/LBXYF56Q==</wsse:BinarySecurityToken><Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\"><SignedInfo><CanonicalizationMethod Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\" /><SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256\" /><Reference URI=\"#TS-008fc455-74f1-4276-a059-5a557ea8a319\"><DigestMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#sha1\" /><DigestValue>jPyIV0IZVmfX4/v7AHA6DrNp4f0=</DigestValue></Reference><Reference URI=\"#id-c49f5d4e-13b4-4ad1-b4ca-42ab885c361b\"><Transforms><Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\"><InclusiveNamespaces PrefixList=\"\" xmlns=\"http://www.w3.org/2001/10/xml-exc-c14n#\" /></Transform></Transforms><DigestMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#sha1\" /><DigestValue>w8pBYe0HPY4OOgZyAeP1RikWQVc=</DigestValue></Reference></SignedInfo><SignatureValue>F4A2OAAHLFhhPqZx/molFQf/TBqABjW88RrVriI/lPLVYtPT7EMkUa5eWZzMne082NLr+yZXb0Ce6uz21lHwOwPe7QovtggFTZw7AgBJkBlvW1CgKpCobM71fWy/vyE02Ekh4gkis2q5OwjWw3bQfEI3HD/EGduuz3yex40iU71twl+xl47wQC1VctpX7l2j5wZiz5GbHg57RWKF3VFRP9ErAUNRx7hmIXKwvv5gh2jrtgoapXzfVhi8RyVs/DATqLJKo/5WWSgUd5PLEiVl0F/UzCSnUKFHN+ET9hn/C3RkHBghhucf7fb1j7FaWMaL5vK3/llU8Fh+YgCodFCDAg==</SignatureValue><KeyInfo Id=\"KS-65ebcfc5-37e1-425a-ab23-b7ccc013894f\"><wsse:SecurityTokenReference wsu:Id=\"STR-4cf359f5-a0a8-4d48-b199-7d6bde22041a\"><Reference URI=\"#X509-e068095a-3fa9-470d-92f1-e000ea273fa9\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\" xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" /></wsse:SecurityTokenReference></KeyInfo></Signature></wsse:Security></soap:Header><soap:Body wsu:Id=\"id-c49f5d4e-13b4-4ad1-b4ca-42ab885c361b\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\"><ns:HentPersonerForespoersel xmlns:ns=\"http://kontaktinfo.difi.no/xsd/oppslagstjeneste/16-02\"><ns:informasjonsbehov>Sertifikat</ns:informasjonsbehov><ns:informasjonsbehov>Kontaktinfo</ns:informasjonsbehov><ns:informasjonsbehov>SikkerDigitalPost</ns:informasjonsbehov><ns:personidentifikator>{ugyldigPersonnummer}</ns:personidentifikator></ns:HentPersonerForespoersel></soap:Body></soap:Envelope>";
            const string forventetWarning = "The 'http://kontaktinfo.difi.no/xsd/oppslagstjeneste/16-02:personidentifikator' element is invalid - The value '121245361221' is invalid according to its datatype 'http://begrep.difi.no:personidentifikator' - The actual length is not equal to the specified length.\n";

            XmlValidator validator = new XmlvalidatorImpl();
            var validateResult = validator.Validate(testRequestMedUgyldigPersonnummer);
            var warnings = validator.ValidationWarnings;

            Assert.AreEqual(forventetWarning, warnings);
            Assert.IsFalse(validateResult,"Burde være negativt resultat på valideringen.");
        }

        public class Navnerom
        {
            public const string OppslagstjenesteDefinisjon = "http://kontaktinfo.difi.no/xsd/oppslagstjeneste/16-02";
            public const string OppslagstjenesteMetadata = "http://begrep.difi.no";

            public const string WssecuritySecext10 =
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";

            public const string WssecurityUtility10 =
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

            public const string SoapEnvelope12 = "http://www.w3.org/2003/05/soap-envelope";
            public const string XmlDsig = "http://www.w3.org/2000/09/xmldsig#";
            public const string XmlExcC14N = "http://www.w3.org/2001/10/xml-exc-c14n#";
            public const string XmlNameSpace = "http://www.w3.org/XML/1998/namespace";
        }

        public class XmlvalidatorImpl : XmlValidator
        {
            private static readonly ResourceUtility ResourceUtility =
                new ResourceUtility("Difi.Felles.Utility.Tester.Testdata.Xsd");

            public XmlvalidatorImpl()
            {
                AddXsd(Navnerom.OppslagstjenesteDefinisjon, HentRessurs("oppslagstjeneste-ws-16-02.xsd"));
                AddXsd(Navnerom.OppslagstjenesteMetadata, HentRessurs("oppslagstjeneste-metadata-16-02.xsd"));
                AddXsd(Navnerom.WssecuritySecext10, HentRessurs("wssecurity.oasis-200401-wss-wssecurity-secext-1.0.xsd"));
                AddXsd(Navnerom.WssecurityUtility10, HentRessurs("wssecurity.oasis-200401-wss-wssecurity-utility-1.0.xsd"));
                AddXsd(Navnerom.SoapEnvelope12, HentRessurs("soap.soap.xsd"));
                AddXsd(Navnerom.XmlDsig, HentRessurs("w3.xmldsig-core-schema.xsd"));
                AddXsd(Navnerom.XmlExcC14N, HentRessurs("w3.exc-c14n.xsd"));
                AddXsd(Navnerom.XmlNameSpace, HentRessurs("w3.xml.xsd"));
            }

            private static XmlReader HentRessurs(string path)
            {
                var bytes = ResourceUtility.ReadAllBytes(true, path);
                return XmlReader.Create(new MemoryStream(bytes));
            }
        }
    }
}