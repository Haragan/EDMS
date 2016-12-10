using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDMS.Models {
    public class DocumentSatus {

        public static string INIT = "INIT";
        public static string CREATED = "Создан";
        public static string SEND_TO_MODERATOR = "Отправлен модератору";
        public static string EDITING = "Редактируется";
        public static string CONFIRMED = "Подтвержден";
        public static string REJECTED = "Отклонен";
        public static string READY = "Готов";

        private DocumentSatus() { }
    }
}