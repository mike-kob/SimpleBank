import views.ReadCardView;
import views.StartView;
import views.View;

import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.HashMap;

public class Session {

    View currentView = null;

    private final HashMap<String, ActionListener> listeners = new HashMap<>();
    private JLayeredPane jpane = null;

    public Session(JLayeredPane jp){
        initListerners();
        this.jpane = jp;
        currentView = new StartView(jp, listeners);
        currentView.init();
    }

    private void changeView(View newView){
        currentView.disposeView();
        currentView = newView;
        currentView.init();
    }

    private void initListerners(){
        listeners.put("start_button", start);
        listeners.put("proceed_enter_card_button", proceed_enter_card);
    }

    private ActionListener start = e -> {
       changeView(new ReadCardView(jpane, listeners));
    };

    private ActionListener proceed_enter_card = e -> {

    };
}
