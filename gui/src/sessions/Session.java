package sessions;

import views.*;

import javax.swing.*;
import java.awt.event.ActionListener;
import java.util.HashMap;

public class Session {

    View currentView = null;

    private final HashMap<String, ActionListener> listeners = new HashMap<>();
    private JLayeredPane jpane = null;

    public Session(JLayeredPane jp){
        initListeners();
        this.jpane = jp;
        changeView(new ReadCardView(jpane, listeners));
    }

    private void changeView(View newView){
        if (currentView != null)
            currentView.disposeView();
        currentView = newView;
        currentView.init();
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
    }

    private ActionListener proceed_enter_card = e -> {
        changeView(new EnterPinView(jpane, listeners));
    };

    private ActionListener confirm_pin = e -> {
        changeView(new OptionsView(jpane, listeners));
    };

    private ActionListener cancel = e -> {
        changeView(new EnterPinView(jpane, listeners));
    };

    private ActionListener change_pin = e -> {
        changeView(new ChangePinView(jpane, listeners));
    };

    private ActionListener withdraw_cash = e -> {
        changeView(new WithdrawCashView(jpane, listeners));
    };

    private ActionListener view_balance = e -> {
        changeView(new DisplayBalanceView(jpane, listeners));
    };

    private ActionListener transfer = e -> {
        changeView(new TransferView(jpane, listeners));
    };

    private ActionListener finish = e -> {
        changeView(new ReadCardView(jpane, listeners));
    };

    private ActionListener confirm_new_pin = e -> {
        changeView(new EnterPinView(jpane, listeners));
    };

    private ActionListener confirm_withdrawal = e -> {
        changeView(new EnterPinView(jpane, listeners));
    };
}
