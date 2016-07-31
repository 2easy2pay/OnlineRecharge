/// <reference path="app.js" />

//Mapping from KD to dollar, DollerAmount is the column name which receive the mappped value.
app.filter('DollerAmount', function () {
    return function (Amount) {
        if (Amount == 2)
            Amount = 5;
        else if (Amount == 3.5)
            Amount = 10;
        else if (Amount == 5)
            Amount = 15;
        else if (Amount == 5.5)
            Amount = 15;
        else if (Amount >= 6 &&  Amount <= 8)
            Amount = 20;
        else if (Amount > 8 && Amount <= 9)
            Amount = 25;
        else if (Amount == 10)
            Amount = 30;
        else if (Amount >= 15 && Amount <= 18)
            Amount = 50;
        else if (Amount == 24)
            Amount = 75;
        else if (Amount >= 32 && Amount <= 33)
            Amount = 100;
        else if (Amount == 64)
            Amount = 200;

        return Amount;
    }
});