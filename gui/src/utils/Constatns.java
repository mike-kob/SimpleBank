package utils;

import java.awt.*;

public class Constatns {

    public static String CARD_ENTER_TEXT = "Please, enter card number and press Enter";

    public static Font TITLE_FONT = new Font("Calibri", Font.PLAIN, 42);
    public static Font DIGIT_FONT = new Font("Arial", Font.PLAIN, 50);

    public static String HOST = "http://localhost:5000/";
    public static String WITHDRAW_URL = HOST + "api/withdraw/";
    public static String BALANCE_URL = HOST + "api/balance/";
    public static String CHANGE_PIN_URL = HOST + "api/changePin/";
    public static String CONFIRM_WITHDRAW_URL = HOST + "api/confirmWithdraw/";
    public static String CARD_EXISTS_URL = HOST + "api/cardExists/";
    public static String TRANSFER_URL = HOST + "api/transfer/";
    public static String START_SESSION_URL = HOST + "api/startSession/";
    public static String LOGIN_URL = HOST + "api/startSession/";



}
