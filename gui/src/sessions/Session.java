package sessions;

import ATM.ATM;
import api.CardAPI;
import api.CardAPIINterface;
import api.UserAPI;
import api.UserAPIInterface;
import utils.HttpHelper;
import views.*;


import javax.swing.*;
import java.awt.event.ActionListener;
import java.util.HashMap;

public class Session {

    private CardAPIINterface cardAPI = new CardAPI();
    private UserAPIInterface userAPI = new UserAPI();

    private String curCard = "";
    private String curPin = "";

    View currentView = null;

    private final HashMap<String, ActionListener> listeners = new HashMap<>();
    private JLayeredPane jpane = null;

    public Session(){
        initListeners();
    }

    public CardAPIINterface getCardAPIClient() { return cardAPI;}

    public UserAPIInterface getUserAPIClient() { return userAPI;}

    public void show() {
        changeView(new ReadCardView(this, jpane, listeners));
    }

    public void setJpane(JLayeredPane jpane) {
        this.jpane = jpane;
    }

    private void changeView(View newView){
        if (currentView != null)
            currentView.disposeView();
        currentView = newView;
        currentView.init();
    }

    public void goToPin() {
        changeView(new EnterPinView(this, jpane, listeners));
    }

    public String getCardNum()
    {
        return curCard;
    }

    public String getCardPin()
    {
        return curPin;
    }

    public void setCardNum(String num)
    {
        curCard = num;
    }

    public void setCardPin(String pin)
    {
        curPin = pin;
    }


    private void initListeners(){
        listeners.put("proceed_enter_card_button", proceed_enter_card);
        listeners.put("confirm_pin_button", confirm_pin);
        listeners.put("cancel_button", cancel);
        listeners.put("change_pin_button", change_pin);
        listeners.put("withdraw_cash_button", withdraw_cash);
        listeners.put("view_balance_button", view_balance);
        listeners.put("transfer_button", transfer);
        listeners.put("finish_button", finish);
        listeners.put("confirm_new_pin_button", confirm_new_pin);
        listeners.put("confirm_withdrawal_button", confirm_withdrawal);
        listeners.put("finish_session", finish_session);
    }


    private ActionListener proceed_enter_card = e -> {
        boolean accepted = cardAPI.exists(this, this.getCardNum());
        if (accepted)
            changeView(new EnterPinView(this, jpane, listeners));
        else
            JOptionPane.showMessageDialog(jpane, "Card is not valid!");
    };

    private ActionListener confirm_pin = e -> {
        boolean accepted = userAPI.login(this);
        if (accepted)
            changeView(new OptionsView(jpane, listeners));
        else
            JOptionPane.showMessageDialog(jpane, "The PIN is incorrect.");
    };

    private ActionListener cancel = e -> {
        changeView(new EnterPinView(this, jpane, listeners));
    };

    private ActionListener change_pin = e -> {
        changeView(new ChangePinView(this, jpane, listeners));
    };

    private ActionListener withdraw_cash = e -> {
        changeView(new WithdrawCashView(jpane, listeners));
    };

    private ActionListener view_balance = e -> {
        changeView(new DisplayBalanceView(this, jpane, listeners));
    };

    private ActionListener transfer = e -> {
        changeView(new TransferView(jpane, listeners));
    };

    private ActionListener finish = e -> {
        changeView(new ReadCardView(this, jpane, listeners));
    };

    private ActionListener confirm_new_pin = e -> {
        changeView(new EnterPinView(this, jpane, listeners));
    };

    private ActionListener confirm_withdrawal = e -> {
        changeView(new EnterPinView(this, jpane, listeners));
    };

    private ActionListener finish_session = e -> {
        SessionManager.finishSession();
    };
}
