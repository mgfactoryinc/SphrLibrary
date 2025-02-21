package sphrlibrary.utilities;

import java.io.File;
import java.nio.file.Path;
import java.nio.file.Paths;

/**
 * ユニットテストのリソースに関するユーティリティ
 */
public class TestResourceUtility {

    /**
     * テスト用のリソースファイルを取得する
     *
     * @param fileName リソースファイル名 (src/test/resources 配下)
     * @return リソースファイル
     */
    public static File getResourceFile(String fileName) {
        Path resourceFilePath = Paths.get("src", "test", "resources", fileName);
        return new File(resourceFilePath.toString());
    }

}
