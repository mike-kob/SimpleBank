from wheels import Wheels


class Bot:

    def __init__(self):
        self.wheels = Wheels()
        pass

    def move(self, left, right, time):
        self.wheels.move(left, right, time)
