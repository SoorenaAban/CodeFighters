#435662

class Game:
    def __init__(self, isVsAI):
        self.health = 40

    def accept_answer(self, answer, player):
        #'one' for player one
        #'two' for player two
        return "test"

    def turn(self):
        return "next question"

    def gameOver(self):
        return True

    def winner(self):
        #return 4 for stalemate
        #return 3 for player 1 win
        #return 2 for player 2 win
        #return 1 for game not over
        return 1

    def getHealth(self, player):
        return self.health
    
    def start(self):
        return True