package com.ghs.pumg.controller;

import com.ghs.pumg.dto.request.AeroCraftRecordRankingReq;
import com.ghs.pumg.dto.request.TitleLockOnReq;
import com.ghs.pumg.dto.request.TitleSetReq;
import com.ghs.pumg.dto.response.AeroCraftRankingRes;
import com.ghs.pumg.dto.response.BaseResponseBody;
import com.ghs.pumg.dto.response.DataResponseBody;
import com.ghs.pumg.dto.response.UserTitleListRes;
import com.ghs.pumg.service.AeroCraftService;
import com.ghs.pumg.service.TitleService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@Slf4j
@CrossOrigin(origins = "*", maxAge = 3600)
@RequiredArgsConstructor
@RestController
@RequestMapping("/api/v1/title")
public class TitleController {

    final TitleService titleService;

    // 600점 이상 획득시 타이틀 저장
    @PostMapping("/aerocraft/lockon/good")
    public ResponseEntity<?> recordLockOnGood(@RequestBody TitleLockOnReq titleInfo) {
        BaseResponseBody res = titleService.recordLockOnGood(titleInfo);
        return ResponseEntity.ok().body(res);
    }
    // 만점 획득시 타이틀 저장
    @PostMapping("/aerocraft/lockon/perfect")
    public ResponseEntity<?> recordLockOnPerfect(@RequestBody TitleLockOnReq titleInfo) {
        BaseResponseBody res = titleService.recordLockOnPerfect(titleInfo);
        return ResponseEntity.ok().body(res);
    }
    @PostMapping("/aerocraft/skyrush")
    public ResponseEntity<?> recordSkyRush(@RequestBody TitleLockOnReq titleInfo) {
        BaseResponseBody res = titleService.recordSkyRush(titleInfo);
        return ResponseEntity.ok().body(res);
    }
    // 개인별 타이틀 목록 가져오기
    @GetMapping("/list/{userIdx}")
    public ResponseEntity<?> getTitleList(@PathVariable("userIdx") Long userIdx) {
        UserTitleListRes res = titleService.getTitle(userIdx);
        return ResponseEntity.ok().body(res);
    }
    @PutMapping("/set/{userIdx}")
    public ResponseEntity<?> setTitle(@RequestBody TitleSetReq titleInfo, @PathVariable("userIdx") Long userIdx) {
        BaseResponseBody res = titleService.setTitle(titleInfo);
        return ResponseEntity.ok().body(res);
    }
    // 타이틀획득
    @PostMapping("/earn")
    public ResponseEntity<?> earnTitle(@RequestBody TitleSetReq titleInfo) {
        BaseResponseBody res = titleService.earnTitle(titleInfo);
        return ResponseEntity.ok().body(res);
    }
    // 타이틀 존재 여부 체크

    @GetMapping("/check/{userIdx}")
    public ResponseEntity<?> checkTitle(@RequestBody TitleSetReq titleInfo, @PathVariable("userIdx") Long userIdx) {
        BaseResponseBody res = titleService.checkTitle(titleInfo);
        return ResponseEntity.ok().body(res);
    }
//    @GetMapping("/get-rank/race")
//    public ResponseEntity<?> getRacingRanking() {
//        AeroCraftRankingRes res = aeroCraftService.getRaceRanking();
//        return ResponseEntity.ok().body(res);
//    }
}
