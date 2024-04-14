package com.tratnjek.maintenancescheduling;

import io.quarkus.test.junit.QuarkusTest;
import org.junit.jupiter.api.Test;

import static io.restassured.RestAssured.given;

@QuarkusTest
public class MaintenanceResourceTest {

    /*@Test
    void testListMaintenanceTasks(){

        given()
                .when().get("/maintenance")
                .then()
                .statusCode(200);
    }*/

   /* @Test
    void testGetMaintenanceTask() {
        // Define the maintenance task to be posted
        Maintenance maintenance = new Maintenance();
        maintenance.name = "Test maintenance";
        maintenance.status = "Test status";
        maintenance.description = "Test description";
        maintenance.location = "Test location";
        maintenance.duration = "Test duration";
        maintenance.type = "Test type";
        maintenance.priority = "Test priority";
        maintenance.notes = "Test notes";

        // POST the maintenance task and extract the ID from the response
        Response postResponse = given()
                .contentType("application/json")
                .body(maintenance)
                .when().post("/maintenance")
                .then()
                .statusCode(200)
                .extract().response();
        Long id = postResponse.jsonPath().getLong("id");

        // GET the maintenance task using the extracted ID
        given()
                .when().get("/maintenance/" + id)
                .then()
                .statusCode(200)
                .body("name", is(maintenance.name))
                .body("status", is(maintenance.status))
                .body("description", is(maintenance.description))
                .body("location", is(maintenance.location))
                .body("duration", is(maintenance.duration))
                .body("type", is(maintenance.type))
                .body("priority", is(maintenance.priority))
                .body("notes", is(maintenance.notes));
    }*/

}
