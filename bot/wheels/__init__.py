import threading
from wheels.Motors import Motors
# from Logger import Logger


class Wheels:

    DEFAULT_TIMEOUT = 1

    interrupted = False
    thread = None
    timer = None

    left = 0
    right = 0

    def __init__(self, client='', service_name='', debug=False):
        self.client = client
        self.serviceName = service_name
        # self.logger = Logger("Wheels", debug)
        self.motors = Motors()

    def move(self, left, right, timeout=DEFAULT_TIMEOUT):
        if self.timer is not None: self.timer.cancel()
        if left < -100: left = -100
        if left > 100: left = 100
        if right < -100: left = -100
        if right > 100: left = 100
        self.motors.set_motor(left, right)

        self.left = left
        self.right = right
        if timeout > 0: self.timer = threading.Timer(timeout, self.halt).start()

    def halt(self):
        self.motors.stop()
        # self.logger.info("Stop moving")
        self.left = 0
        self.right = 0
