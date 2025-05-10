LOWER_BOUND = 1
UPPER_BOUND = 100
LOW = "low"
HIGH = "high"
CORRECT = "correct"

def PlayNumberGuessingGame():
    targetNumber= GenerateTargetNumber()
    attempts = 0

    print("Welcome to the Number Guessing Game!")

    while True:
        guess = GetValidUserInput()
        attempts += 1
        feedback = ProvideFeedback(targetNumber, guess)

        if feedback == LOW:
            print("Too low. Try again.")
        elif feedback == High:
            print("Too high. Try again.")
        else:
            print(f"Congratulations! You guessed the number in {attempts} attempts!")
            break



def GenerateTargetNumber():
    return random.randint(LOWER_BOUND, UPPER_BOUND)


def GetValidUserInput():
    while True:
        userInput = input(f"Enter your guess (between {LOWER_BOUND} and {UPPER_BOUND}): ")
        if IsValidGuess(userInput):
            return int(userInput)
        else:
            print(f"Invalid input! Please enter a number between {LOWER_BOUND} and {UPPER_BOUND}.")

def IsValidGuess(inputGuess):
    return inputGuess.isdigit() and LOWER_BOUND <= int(inputGuess) <= UPPER_BOUND


def ProvideFeedback(targetNumber, guess):
    if guess < targetNumber:
        return LOW
    elif guess > targetNumber:
        return HIGH
    
    return CORRECT