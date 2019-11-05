package views;

import sessions.Session;
import utils.Constatns;
import javax.swing.*;
import javax.swing.border.Border;
import java.awt.*;
import java.awt.event.ActionListener;
import java.util.Arrays;
import java.util.HashMap;

public class EnterPinView implements View{
    private final JLayeredPane jpane;
    private final HashMap<String, ActionListener> listeners;
    private JPasswordField password;
    private final Session session;

    public EnterPinView(Session session, JLayeredPane jp, HashMap<String, ActionListener> listeners) {
        this.jpane = jp;
        this.listeners = listeners;
        this.session = session;
    }

    @Override
    public void init() {
        JLabel caption = new JLabel("Enter your PIN-code:");
        caption.setFont(Constatns.TITLE_FONT);
        caption.setHorizontalAlignment(SwingConstants.CENTER);
        caption.setSize(700, 400);
        int cx = (jpane.getWidth() - caption.getWidth()) / 2;
        caption.setLocation(cx, 50);
        caption.setVisible(true);
        jpane.add(caption, 0);

        JPasswordField pinField = new JPasswordField();
        pinField.setBounds((jpane.getWidth() - caption.getWidth()) / 2 + 100,300, 500, 50);
        pinField.setBackground(Color.white);
        pinField.setFont(new Font("Arial", Font.PLAIN, 20));
        pinField.setHorizontalAlignment(SwingConstants.CENTER);
        jpane.add(pinField);
        this.password = pinField;
        addButtons();

        JButton confirm = new JButton("Confirm");
        confirm.setSize(160, 80);
        confirm.setFont(new Font("Arial", Font.PLAIN, 20));
        int px = (jpane.getWidth() - confirm.getWidth()) / 2 - 130;
        confirm.setLocation(px, 900);
        confirm.setVisible(true);
        //confirm.setContentAreaFilled(false);
        //confirm.setBorder(new RoundedBorder(25));
        confirm.addActionListener(listeners.get("confirm_pin_button"));
        confirm.addActionListener(e -> this.session.setCardPin(pinField.getText()));
        jpane.add(confirm, 0);

        JButton cancel = new JButton("Cancel");
        cancel.setSize(160, 80);
        cancel.setFont(new Font("Arial", Font.PLAIN, 20));
        cancel.setLocation(px + 250, 900);
        cancel.setVisible(true);
        cancel.addActionListener(listeners.get("finish_session"));
        jpane.add(cancel, 0);
        jpane.repaint();

        pinField.requestFocus();
    }

    @Override
    public void disposeView() {
        jpane.removeAll();
        jpane.repaint();
    }

    private void addButtons()
    {
        int width = 90;
        int height = 90;
        int px = (jpane.getWidth() - width) / 2 - 120;

        Rectangle r1 = new Rectangle(px, 400, width,height);
        addButton("1", r1);

        Rectangle r2 = new Rectangle(px + 115, 400, width, height);
        addButton("2", r2);

        Rectangle r3 = new Rectangle(px + 230, 400, width, height);
        addButton("3", r3);

        Rectangle r4 = new Rectangle(px, 515, width, height);
        addButton("4", r4);

        Rectangle r5 = new Rectangle(px + 115, 515, width, height);
        addButton("5", r5);

        Rectangle r6 = new Rectangle(px + 230, 515, width, height);
        addButton("6", r6);

        Rectangle r7 = new Rectangle(px, 630, width, height);
        addButton("7", r7);

        Rectangle r8 = new Rectangle(px + 115, 630, width, height);
        addButton("8", r8);

        Rectangle r9 = new Rectangle(px + 230, 630, width, height);
        addButton("9", r9);

        Rectangle r0 = new Rectangle(px + 115, 745, width, height);
        addButton("0", r0);

        jpane.repaint();
    }

    private void addButton(String text, Rectangle bounds) {
        JButton button = new JButton(text);
        button.setFont(Constatns.DIGIT_FONT);
        button.setBounds(bounds);
        button.setVisible(true);
        button.addActionListener(e -> addDigit(text));
        jpane.add(button, 0);
    }

    private void addDigit(String digit) {
        String cur = this.password.getText();
        this.password.setText(cur + digit);
        jpane.repaint();
    }
}
