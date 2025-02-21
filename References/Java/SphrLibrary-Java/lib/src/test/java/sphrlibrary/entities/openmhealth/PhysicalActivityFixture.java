package sphrlibrary.entities.openmhealth;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;

import java.io.IOException;

import org.junit.jupiter.api.Test;

import com.fasterxml.jackson.databind.ObjectMapper;

import lombok.val;
import sphrlibrary.utilities.TestResourceUtility;

public class PhysicalActivityFixture {

    @Test
    public void PhysicalActivityデータをデシリアライズできる() {

        // Arrange
        // Jackson の ObjectMapper を生成 & テスト用の JSON ファイルを取得
        val mapper = new ObjectMapper();
        val jsonFile = TestResourceUtility.getResourceFile("physical-activity_001.json");
        PhysicalActivity physicalActivity = null;

        // Act
        // JSON ファイルを PhysicalActivity クラスにデシリアライズ
        try {
            physicalActivity = mapper.readValue(jsonFile, PhysicalActivity.class);
        } catch (IOException e) {
            e.printStackTrace();
            throw new RuntimeException(e);
        }

        // Assert
        // デシリアライズした PhysicalActivity オブジェクトの各プロパティが正しいことを確認
        assertNotNull(physicalActivity);

        // ヘッダー
        val header = physicalActivity.getHeader();
        assertNotNull(header);
        val acquistionRate = header.getAcquisitionRate();
        assertNotNull(acquistionRate);
        assertEquals("steps", acquistionRate.getUnit());
        assertEquals(1, acquistionRate.getValue());
        val externalDataSheets = header.getExternalDataSheets();
        assertNotNull(externalDataSheets);
        assertEquals(0, externalDataSheets.size());
        assertEquals("self_reported", header.getModality());
        val schemaID = header.getSchemaID();
        assertNotNull(schemaID);
        assertEquals("ieee", schemaID.getNamespace());
        assertEquals("physical-activity", schemaID.getName());
        assertEquals("1.0", schemaID.getVersion());
        assertEquals("2024-12-20T14:01:19.4112289+09:00", header.getSourceCreationDateTime());
        assertEquals("26bc9a02-d5f6-421b-b709-13e517c671c9", header.getUuid());

        // ボディ
        val bodies = physicalActivity.getBody();
        assertNotNull(bodies);
        assertEquals(1, bodies.size());
        val body = bodies.get(0);
        assertNotNull(body);
        val effectiveTimeFrame = body.getEffectiveTimeFrame();
        assertNotNull(effectiveTimeFrame);
        val timeInterval = effectiveTimeFrame.getTimeInterval();
        assertNotNull(timeInterval);
        assertEquals("2024-12-01T00:00:00.0000000+09:00", timeInterval.getStartDateTime());
        assertEquals("2024-12-01T23:59:59.9990000+09:00", timeInterval.getEndDateTime());
        assertEquals("Walking", body.getActivityName());
        val baseMovementQuantity = body.getBaseMovementQuantity();
        assertNotNull(baseMovementQuantity);
        assertEquals("steps", baseMovementQuantity.getUnit());
        assertEquals(5000, baseMovementQuantity.getValue());
        val distance = body.getDistance();
        assertNotNull(distance);
        assertEquals("km", distance.getUnit());
        assertEquals(1.8, distance.getValue());
    }

}
