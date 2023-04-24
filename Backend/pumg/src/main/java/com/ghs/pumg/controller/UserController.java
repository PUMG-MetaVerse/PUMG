package com.ghs.pumg.controller;

import com.ghs.pumg.dto.request.UserHealingCharacterReq;
import com.ghs.pumg.dto.request.UserLoginReq;
import com.ghs.pumg.dto.request.UserSignUpReq;
import com.ghs.pumg.dto.request.UserWorldCharacterReq;
import com.ghs.pumg.dto.response.*;
import com.ghs.pumg.service.UserService;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@Slf4j
@CrossOrigin(origins = "*", maxAge = 3600)
@RequiredArgsConstructor
@RestController
@RequestMapping("/api/v1/user")
public class UserController {

    final UserService userService;

    // 회원 가입
    @PostMapping("/signup")
    public ResponseEntity<?> signUp(@RequestBody UserSignUpReq signUpInfo) {
        //임의로 리턴된 User 인스턴스. 현재 코드는 회원 가입 성공 여부만 판단하기 때문에 굳이 Insert 된 유저 정보를 응답하지 않음.
        UserSignUpRes res = userService.signUpUser(signUpInfo);
        return ResponseEntity.ok().body(res);
    }

    // 로그인
    @PostMapping("/login")
    public ResponseEntity<?> login(@RequestBody UserLoginReq signUpInfo) {
        //임의로 리턴된 User 인스턴스. 현재 코드는 회원 가입 성공 여부만 판단하기 때문에 굳이 Insert 된 유저 정보를 응답하지 않음.
        UserLoginRes res = userService.loginUser(signUpInfo);
        return ResponseEntity.ok().body(res);
    }
    // 캐릭터 선택
    @PostMapping("/set-world-character")
    public ResponseEntity<?> setWorldCharacter(@RequestBody UserWorldCharacterReq worldInfo) {
        //임의로 리턴된 User 인스턴스. 현재 코드는 회원 가입 성공 여부만 판단하기 때문에 굳이 Insert 된 유저 정보를 응답하지 않음.
        UserWorldCharacterRes res = userService.setWorldCharacter(worldInfo);
        return ResponseEntity.ok().body(res);
    }
    // 캐릭터 선택
    @PostMapping("/set-healing-character")
    public ResponseEntity<?> setHealingCharacter(@RequestBody UserHealingCharacterReq HealingInfo) {
        //임의로 리턴된 User 인스턴스. 현재 코드는 회원 가입 성공 여부만 판단하기 때문에 굳이 Insert 된 유저 정보를 응답하지 않음.
        UserHealingCharacterRes res = userService.setHealingCharacter(HealingInfo);
        return ResponseEntity.ok().body(res);
    }
}
