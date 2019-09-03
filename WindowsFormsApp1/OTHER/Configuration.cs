using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatBalakovo.OTHER
{
    public  class Configuration
    {
        public const string textIdUnknown = " * Для того, что бы, начать общаться, необходимо зарегистрироватся в чате &#128540;\nПросто введи команду: \n!рег ник \nНапример: !рег ВасяПупкин";
        public const string textCmdUnknown = " * Такой команды не существует. Что бы посмотреть все команды, используйте: \n!помощь";
        public const string nicknameAleardy = " * Этот ник уже занят, придумайте другой.";
        public const string textRegistrationSuccesfull = " * Благодарим за регистрацию. \nДля того что бы начать общение, введите команду:\n!начать";
        public const string textUserIsBanned = " * Вы забанены до:";
        public const string textUserIsExistRegistration = " * Вы уже зарегистрированы в чате &#128529;";
        public const string textUserIsOffline = " * Привет! &#9995; \nЧто бы войти в чат, отправьте команду: \n!начать";
        public const string textUserIsNoActivity = " * Вы слишком долго бездействовали, был произведен автоматический выход из чата" +
            "\nЧто бы запустить чат отправте команду: " +
            "\n!начать";
        public const string textUserIsBeginChat = "Добро пожаловать &#128515; " +
            "\nВы вошли в чат. Что бы посмотреть список всех команд" +
            "\nиспользуйте команду: !помощь" +
            "\nдля выхода из чата: !выход";
        public const string textChangeNicknameIsComplited = "сменил ник на: ";
        public const string textHelpCmd = "Общие команды:" +
            "\n\"!ник новыйник\" - Сменить ник, например !ник Вася" +
            "\n\"!ктотут\" - Показать всех кто сейчас в чате" +
            "\n\"!выход\" - Выйти из чата" +
            "\n\"!начать\" - Войти в чат" +
            "\n\nКоманды для модераторов:" +
            "\n\"!инфо 0\" - Посмотреть инфу о пользователе, вместо нуля взять нужную цифру из списка \"!ктотут\"" +
            "\n\"!бан 0 0\" - Заблокировать пользователя, вместо первого нуля взять нужную цифру из списка \"!ктотут\", " +
            "вместо второго нуля, цифру, на какое кол-во дней блокировать: 1, 7 и 30";
        public const string textExitUser = " * Вы вышли из чата." +
            "\nЧто бы вернутся, команда: \n!начать";

        public const int timeWaitNoActiveUser = 60; //максимальное время бездействия юзера, мин.
        public const int timeWaitCheckIsNoActiveUser = 5; //интервал через которое таймер будет чекать активность, мин
        public const string nameFileChatLastMsg = @"\RESOURSES\lastMsg.txt"; //имя файла в последними сообщениямси чата
        public const string databaseName = @"\RESOURSES\data.db"; // БД нейм
        public const int maxCountLastMessageChat = 5; // максимальное кол-во вывода при входе последних сообщений чата


        public enum ColumnNameTableOnline
        {
            Idvk,
            Lastmsg
        };

        public enum ColumnNameTableData //idvk,nickname,accesslvl,bantodate,banreason,note
        {
            Idvk,
            Nickname,
            Accesslvl,
            Bantodate,
            Banreason,
            Note
        };

        public enum Counters //
        {
            oneSend,
            onlineSend,
            allSend
           
        };

    }
}
