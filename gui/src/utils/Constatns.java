package utils;

import java.awt.*;

public class Constatns {

    public static String CARD_ENTER_TEXT = "Please, enter card number and press Enter";

    public static Font TITLE_FONT = new Font("Calibri", Font.PLAIN, 32);
    public static Font DIGIT_FONT = new Font("Arial", Font.PLAIN, 50);
    public static Font BUTTON_FONT = new Font("Arial", Font.PLAIN, 20);
    public static boolean VIRTUAL = false;

    public static String HOST = "http://localhost:5000/";
    public static String WITHDRAW_URL = "api/withdraw/";
    public static String BALANCE_URL = "api/balance/";
    public static String CHANGE_PIN_URL = "api/changePin/";
    public static String CONFIRM_WITHDRAW_URL = "api/confirmWithdraw/";
    public static String CARD_EXISTS_URL = "api/cardExists/";
    public static String TRANSFER_URL = "api/transfer/";
    public static String START_SESSION_URL = "api/startSession/";
    public static String LOGIN_URL = "api/login/";

    public static int CANCEL_KEY = 27;

    public static int INACTIVITY_SECONDS = 30;

}
