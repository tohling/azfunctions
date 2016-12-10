var util = require('util');

module.exports = function (context, myEventHubTrigger) {
    context.log(util.format("JavaScript eventhub trigger function processed %d events", myEventHubTrigger.length));

    for (i = 0; i < myEventHubTrigger.length; i++)
    {
        context.log(myEventHubTrigger[i]);
    }

    context.done();
}