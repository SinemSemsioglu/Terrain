fs = require('fs');

saveConfig = (data) => {
    // only supporting sync read-write is not good at all (only one op at a time), 
    // but might not be a big problem with small amount of users
    // otherwise need to properly deal with concurrent operations;
    var terrains = JSON.parse(fs.readFileSync('./terrains.json'));
    console.log(data);
    if (terrains.hasOwnProperty(data.terrainName)) {
        // already exists
        return {success: false, error: "a terrain with this name already exists"}
    } else {
        terrains[data.terrainName] = data; // terrain name is repetitive and password is not hashed
        fs.writeFileSync('./terrains.json', JSON.stringify(terrains));
        return {success: true}
    }

    /*
    return new Promise((resolve, reject) => {
        // for now write to a JSON file



        
        var dataToInsert = processData(data);
        knex('terrain_config').insert(dataToInser)
        .then(([configId]) => {
            // add path locations to another file
          })
          .catch((err) => {
             
          }); 
    }) */
     
}

retrieveConfig = (terrainName, terrainPass) => {
    console.log(terrainName + " " + terrainPass);
    var terrains = JSON.parse(fs.readFileSync('./terrains.json'));
    if (terrains.hasOwnProperty(terrainName)) {
        // already exists
        if (terrains[terrainName].terrainPass == terrainPass) {
            return terrains[terrainName];
        } else {
            return {success: false, error: "password doesn't match"};
        }
       
    } else {
        return {success: false, error: "no terrain found with this name"};
    }
}

retrieveConfigWithoutPass = (terrainName, terrainPass) => {
    console.log(terrainName + " " + terrainPass);
    var terrains = JSON.parse(fs.readFileSync('./terrains.json'));
    if (terrains.hasOwnProperty(terrainName)) {
        // already exists
        return terrains[terrainName];
    } else {
        return {success: false, error: "no terrain found with this name"};
    }
}

retrieveAllTerrainNames = () => {
    console.log(terrainName + " " + terrainPass);
    var terrains = JSON.parse(fs.readFileSync('./terrains.json'));
    return {names: Object.keys(terrains)};
}


processData = (data) => {
    var processed = {
        name: data.name,
        password: data.password,
        athmosphere_thickness: data.athmosphereThickness,
        sea_color_h: data.seaColor.x,
        sea_color_s: data.seaColor.y,
        sea_color_v: data.sea_color.z,
        height_relations_v_d: data.heightRelations[0],
        height_relations_v_i: data.heightRelations[1],
        height_relations_a_d: data.heightRelations[2],
        noise_relations_a_i: data.heightRelations[3],
        noise_relations_v_d: data.noiseRelations[0],
        noise_relations_v_i: data.noiseRelations[1],
        noise_relations_a_d: data.noiseRelations[2],
        noise_relations_a_i: data.noiseRelations[3]
      };

    for (var i=0; i <data.lightHSV.length; i++) {
        var key = "light_" + i.toString() + "_color";
        processed[key + "_h"] = data.lightHSV[i].x;
        processed[key + "_s"] = data.lightHSV[i].y;
        processed[key + "_v"] = data.lightHSV[i].z; 
    }

    return processed;
}

module.exports = {
    saveConfig: saveConfig,
    retrieveConfig: retrieveConfig,
    retrieveAllTerrainNames: retrieveAllTerrainNames,
    retrieveConfigWithoutPass: retrieveConfigWithoutPass
}