package sphrlibrary.entities.openmhealth;

import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import com.fasterxml.jackson.databind.annotation.JsonNaming;

import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.NonNull;

/**
 * Open mHealth の SchemaId Entity
 */
@Data
@NoArgsConstructor
@JsonNaming(PropertyNamingStrategies.SnakeCaseStrategy.class)
public class SchemaId {

    /**
     * スキーマの名前空間
     */
    @NonNull
    private String namespace = "";

    /**
     * スキーマの名前
     */
    @NonNull
    private String name = "";

    /**
     * スキーマのバージョン
     */
    @NonNull
    private String version = "";
}
