class Game:
    def __init__(self, isVsAI):
        self.health = 40

    def answer(self, answer, player):
        return "test"

    def turn(self):
        return "test"

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