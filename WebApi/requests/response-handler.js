// console.log(response.body);
// console.log("response.body", response.body);

client.test("Hello, World!", function(){
    client.assert(response.status === 200, "Response status is not 200");
});