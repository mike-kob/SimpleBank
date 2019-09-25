package views;

import utils.Constatns;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionListener;
import java.util.HashMap;

public class ReadCardView implements View {

    private final JLayeredPane jpane;
    private final HashMap<String, ActionListener> listeners;

    public ReadCardView(JLayeredPane jp, HashMap<String, ActionListener> listeners) {
        this.jpane = jp;
        this.listeners = listeners;
    }

    @Override
    public void init() {
        JLabel video = new JLabel();
        video.setBackground(Color.GRAY);
        video.setSize(700, 400);
        int x = (jpane.getWidth() - video.getWidth()) / 2;
        int y = 20;
        video.setLocation(x, y);
        video.setVisible(true);
        video.setOpaque(true);
        jpane.add(video, 0);

        JLabel caption = new JLabel(Constatns.CARD_ENTER_TEXT);
        caption.setFont(Constatns.TITLE_FONT);
        caption.setHorizontalAlignment(SwingConstants.CENTER);
        caption.setSize(700, 400);
        int cx = (jpane.getWidth() - caption.getWidth()) / 2;
        int cy = 500;
        caption.setLocation(cx, cy);
        caption.setVisible(true);
        jpane.add(caption, 0);

        JButton proceed = new JButton();
        proceed.setSize(160, 80);
        int px = (jpane.getWidth() - caption.getWidth()) / 2;
        int py = 500;
        proceed.setLocation(px, py);
        proceed.setVisible(true);
        proceed.addActionListener(listeners.get("proceed_enter_card_button"));

    }

    @Override
    public void disposeView() {
        jpane.removeAll();
        jpane.repaint();
    }
}
