using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDMS.Models {
    public class DocumentSatus {

        public const string INIT = "INIT"; //технический системный статус
        public const string CREATED = "Создан";
        public const string SEND_TO_MODERATOR = "Отправлен модератору";
        public const string EDITING = "Редактируется";
        public const string CONFIRMED = "Подтвержден";
        public const string REJECTED = "Отклонен";
        public const string READY = "Готов";

        private DocumentSatus() { }
    }
}