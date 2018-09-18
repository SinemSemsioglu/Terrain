var bodyParser = require('body-parser');
var express = require('express');
var app = express();
var dbManager = require('./db_manager');


app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

app.set('port', (process.env.PORT || process.env.R_PORT || 5000));

app.use(express.static('./public'));

app.put('/uploadConfig', (req, res) => {
    res.send(dbManager.saveConfig(req.body));
});

app.post('/getConfig', (req, res) => {
    res.send(dbManager.retrieveConfig(req.body.terrainName, req.body.terrainPass));
});

app.get('/getAllTerrainNames', (req, res)=> {
    console.log("req received");
    res.send(dbManager.retrieveAllTerrainNames());
});

app.post('/getTerrainView', (req, res) => {
    res.send(dbManager.retrieveConfigWithoutPass(req.body.terrainName));
})

var server = app.listen(app.get('port'), function () {
  console.log('Node app is running on port', app.get('port'));
});
