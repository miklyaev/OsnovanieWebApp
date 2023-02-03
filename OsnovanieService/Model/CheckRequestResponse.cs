namespace OsnovanieService.Model
{
    public class CertsInFnsInfo
    {
        public string Sn { get; set; }
        public string Issued { get; set; }
    }

    public class RequestInfo
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Company { get; set; }

        public string CompanyFull { get; set; }

        public string Position { get; set; }

        public string PassportSerial { get; set; }

        public string PassportNumber { get; set; }

        public string PassportDate { get; set; }

        public string PassportCode { get; set; }

        public string PassportDivision { get; set; }

        public string Gender { get; set; }

        public string BirthDate { get; set; }

        public string Inn { get; set; }

        public string Ogrn { get; set; }

        public string Snils { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Index { get; set; }

        //public OwnerType Type { get; set; }

        //public List<int> Products { get; set; }

        public bool OfferJoining { get; set; }

        public int IdentificationKind { get; set; }

        #region Для ЮЛ

        public string HeadFirstName { get; set; }

        public string HeadLastName { get; set; }

        public string HeadMiddleName { get; set; }

        public string PersonInn { get; set; }

        public string Kpp { get; set; }

        public string RegionLaw { get; set; }

        public string CityLaw { get; set; }

        public string AddressLaw { get; set; }

        public string Department { get; set; }

        public string CompanyPhone { get; set; }

        public string HeadPosition { get; set; }

        #endregion

        public string Comment { get; set; }
    }
    public enum FoundFnsCertificateCheck
    {
        /// <summary>
        /// Проверка не требуется
        /// </summary>
        NotRequired = 0,
        /// <summary>
        /// Проверка отправлена
        /// </summary>
        Sent = 1,
        /// <summary>
        /// Выпущенные сертификаты не найдены
        /// </summary>
        IssuedCertificatesNotFound = 2,
        /// <summary>
        /// Выпущенные сертификаты найдены
        /// </summary>
        IssuedCertificatesFound = 3,
        /// <summary>
        /// Ошибка проверки сертификата в ФНС
        /// </summary>
        CheckError = 4,
        /// <summary>
        /// Филиал проигнорировал существование сертификата в ФНС
        /// </summary>
        IgnoredByPartner = 5,
        /// <summary>
        /// Действующий сертификат есть, но также есть и заявление на отзыв
        /// </summary>
        FoundAndRevoked = 6,
        /// <summary>
        /// Проверки отменены, выпуск в ФНС принудительно разрешен 
        /// </summary>
        Cancelled = 7
    }
    public class RequestInfoFromResponse : RequestInfo
    {
        public FoundFnsCertificateCheck CheckCertInFNSStatus { get; set; }
        public List<CertsInFnsInfo> ExistingCertsInFNS { get; set; }
    }

    public enum RequestStatusInfo
    {
        None = 0,
        SendDocuments = 1,
        RequestGeneration = 2,
        CertificateIssue = 3,
        Success = 4,
        Deny = 5,
        WaitManagerSign = 11,
        Moderation = 21
    }
    public class CheckRequestResponse
    {
        public RequestInfoFromResponse Info { get; set; }
        public int RequestId { get; set; }
        public RequestStatusInfo StatusId { get; set; }
        public string Status { get; set; }
    }
}
