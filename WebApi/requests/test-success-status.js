client.test("Hello, World!", function(){
    client.assert(response.status >= 200 && response.status <= 299, "Response status is not 200");
});