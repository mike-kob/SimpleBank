package sessions;

import views.EnterPinView;
import views.ReadCardView;
import views.OptionsView;
import views.View;

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
    }

    private ActionListener proceed_enter_card = e -> {
        changeView(new EnterPinView(jpane, listeners));
    };

    private ActionListener confirm_pin = e -> {
        changeView(new OptionsView(jpane, listeners));
    };

    private ActionListener cancel = e -> {
    };

    private ActionListener change_pin = e -> {
    };

    private ActionListener withdraw_cash = e -> {
    };

    private ActionListener view_balance = e -> {
    };

    private ActionListener transfer = e -> {
    };

    private ActionListener finish = e -> {
    };
}
