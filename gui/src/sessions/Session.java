package sessions;

import views.ReadCardView;
import views.View;

import javax.swing.*;
import java.awt.event.ActionListener;
import java.util.HashMap;

public class Session {

    View currentView = null;

    private final HashMap<String, ActionListener> listeners = new HashMap<>();
    private JLayeredPane jpane = null;

    public Session(JLayeredPane jp){
        initListerners();
        this.jpane = jp;
        changeView(new ReadCardView(jpane, listeners));
    }

    private void changeView(View newView){
        if (currentView != null)
            currentView.disposeView();
        currentView = newView;
        currentView.init();
    }

    private void initListerners(){
        listeners.put("proceed_enter_card_button", proceed_enter_card);
    }

    private ActionListener proceed_enter_card = e -> {

    };
}
