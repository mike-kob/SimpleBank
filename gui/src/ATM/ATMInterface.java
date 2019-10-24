package ATM;

import sessions.Session;

public interface ATMInterface {

    void checkoutUnits(Session session, int units);

    int getUnitsLeft(Session session);

    Boolean isCheckoutSuccessful(Session session);


}
