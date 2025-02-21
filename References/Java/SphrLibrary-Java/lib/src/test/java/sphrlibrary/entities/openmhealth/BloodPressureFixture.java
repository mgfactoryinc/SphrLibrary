package sphrlibrary.entities.openmhealth;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;

import java.io.IOException;
import org.junit.jupiter.api.Test;

import com.fasterxml.jackson.databind.ObjectMapper;

import lombok.val;

import sphrlibrary.utilities.TestResourceUtility;

public class BloodPressureFixture {

    @Test
    public void BloodPressureデータをデシリアライズできる() {

        // Arrange
        // Jackson の ObjectMapper を生成 & テスト用の JSON ファイルを取得
        val mapper = new ObjectMapper();
        val jsonFile = TestResourceUtility.getResourceFile("blood-pressure_001.json");
        BloodPressure bloodPressure = null;

        // Act
        // JSON ファイルを BloodPressure クラスにデシリアライズ
        try {
            bloodPressure = mapper.readValue(jsonFile, BloodPressure.class);
        } catch (IOException e) {
            e.printStackTrace();
            throw new RuntimeException(e);
        }

        // Assert
        // デシリアライズした BloodPressure オブジェクトの各プロパティが正しいことを確認
        assertNotNull(bloodPressure);

        // ヘッダー
        val header = bloodPressure.getHeader();
        assertNotNull(header);
        val acquisitionRate = header.getAcquisitionRate();
        assertNotNull(acquisitionRate);
        assertEquals("mmHg", acquisitionRate.getUnit());
        assertEquals(1, acquisitionRate.getValue());
        val externalDataSheets = header.getExternalDataSheets();
        assertNotNull(externalDataSheets);
        assertEquals(0, externalDataSheets.size());
        assertEquals("self_reported", header.getModality());
        val schemaID = header.getSchemaID();
        assertNotNull(schemaID);
        assertEquals("blood-pressure", schemaID.getName());
        assertEquals("omh", schemaID.getNamespace());
        assertEquals("4.0", schemaID.getVersion());
        assertEquals("2024-12-20T14:01:19.4098641+09:00", header.getSourceCreationDateTime());
        assertEquals("28bbc2e7-632e-4bb8-85af-4016195189c1", header.getUuid());

        // ボディ
        val bodies = bloodPressure.getBody();
        assertNotNull(bodies);
        assertEquals(1, bodies.size());
        val body = bodies.get(0);
        assertNotNull(body);
        val effectiveDateTime = body.getEffectiveTimeFrame();
        assertNotNull(effectiveDateTime);
        assertEquals("2024-12-01T00:00:00.0000000+09:00", effectiveDateTime.getDateTime());
        assertEquals("sitting", body.getBodyPosture());
        assertEquals("", body.getDescriptiveStatistic());
        val diastolicBloodPressure = body.getDiastolicBloodPressure();
        assertNotNull(diastolicBloodPressure);
        assertEquals("mmHg", diastolicBloodPressure.getUnit());
        assertEquals(82, diastolicBloodPressure.getValue());
        assertEquals("rightupperarm", body.getMeasurementLocation());
        val systolicBloodPressure = body.getSystolicBloodPressure();
        assertNotNull(systolicBloodPressure);
        assertEquals("mmHg", systolicBloodPressure.getUnit());
        assertEquals(144, systolicBloodPressure.getValue());
        assertEquals("at_rest", body.getTemporalRelationshipToPhysicalActivity());
    }
}
