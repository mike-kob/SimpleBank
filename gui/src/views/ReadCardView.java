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
//        JLabel video = new JLabel();
//        video.setBackground(Color.GRAY);
//        video.setSize(700, 400);
//        int x = (jpane.getWidth() - video.getWidth()) / 2;
//        int y = 20;
//        video.setLocation(x, y);
//        video.setVisible(true);
//        video.setOpaque(true);
//        jpane.add(video, 0);

        JLabel caption = new JLabel(Constatns.CARD_ENTER_TEXT);
        caption.setFont(Constatns.TITLE_FONT);
        caption.setHorizontalAlignment(SwingConstants.CENTER);
        caption.setSize(900, 400);
        int cx = (jpane.getWidth() - caption.getWidth()) / 2;
        int cy = 400;
        caption.setLocation(cx, cy);
        caption.setVisible(true);
        jpane.add(caption, 0);

        JTextField carnNum = new JTextField();
        carnNum.setHorizontalAlignment(SwingConstants.CENTER);
        carnNum.setFont(Constatns.TITLE_FONT);
        carnNum.setSize(700, 70);
        int ty = 400;
        int tx = (jpane.getWidth() - carnNum.getWidth()) / 2;
        carnNum.setLocation(tx, ty);
        carnNum.setVisible(true);
        carnNum.setToolTipText("Card number");
        carnNum.getText();
        jpane.add(carnNum, 0);

        JButton proceed = new JButton("Enter");
        proceed.setSize(160, 80);
        proceed.setFont(new Font("Arial", Font.PLAIN, 20));
        int px = (jpane.getWidth() - proceed.getWidth()) / 2;
        int py = 700;
        proceed.setLocation(px, py);
        proceed.setVisible(true);
        proceed.addActionListener(listeners.get("proceed_enter_card_button"));
        jpane.add(proceed, 0);

        jpane.repaint();

        carnNum.requestFocus();


    }

    @Override
    public void disposeView() {
        jpane.removeAll();
        jpane.repaint();
    }
}
