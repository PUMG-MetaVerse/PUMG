package com.ghs.pumg.controller;

import com.ghs.pumg.dto.request.*;
import com.ghs.pumg.dto.response.*;
import com.ghs.pumg.service.AeroCraftService;
import com.ghs.pumg.service.UserService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.sql.Time;
import java.sql.Timestamp;
import java.text.SimpleDateFormat;

@Slf4j
@CrossOrigin(origins = "*", maxAge = 3600)
@RequiredArgsConstructor
@RestController
@RequestMapping("/api/v1/flight")
public class AeroCraftController {

    final AeroCraftService aeroCraftService;

    // 게임 결과 저장
    @PostMapping("/record-score")
    public ResponseEntity<?> recordScore(@RequestBody AeroCraftRecordRankingReq rankingInfo) {
        BaseResponseBody res = aeroCraftService.recordRanking(rankingInfo);
        return ResponseEntity.ok().body(res);
    }
    @GetMapping("/get-rank")
    public ResponseEntity<?> getRanking() {
        AeroCraftRankingRes res = aeroCraftService.getRanking();
        return ResponseEntity.ok().body(res);
    }
    @GetMapping("/get-rank/race")
    public ResponseEntity<?> getRacingRanking() {
        AeroCraftRankingRes res = aeroCraftService.getRaceRanking();
        return ResponseEntity.ok().body(res);
    }
}
