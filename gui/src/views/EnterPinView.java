package views;

import utils.Constatns;
import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionListener;
import java.util.HashMap;

public class EnterPinView implements View{
    private final JLayeredPane jpane;
    private final HashMap<String, ActionListener> listeners;

    public EnterPinView(JLayeredPane jp, HashMap<String, ActionListener> listeners) {
        this.jpane = jp;
        this.listeners = listeners;
    }

    @Override
    public void init() {

    }

    @Override
    public void disposeView() {
        jpane.removeAll();
        jpane.repaint();
    }
}
