package views;

import utils.LocationHelper;

import javax.swing.*;
import java.awt.event.ActionListener;
import java.util.HashMap;

public class StartView implements View{

    private final JLayeredPane jpane;
    private final HashMap<String, ActionListener> listeners;

    public StartView(JLayeredPane fr, HashMap<String, ActionListener> listeners){
        this.jpane = fr;
        this.listeners = listeners;
    }

    @Override
    public void init() {
        JButton start = new JButton("Start");
        start.setSize(160, 80);
        start.setVisible(true);
        start.setLocation(LocationHelper.centerLocation(jpane, start));
        start.addActionListener(listeners.get("start_button"));
        jpane.add(start,0);
    }

    @Override
    public void disposeView() {
        jpane.removeAll();
        jpane.repaint();
    }
}
