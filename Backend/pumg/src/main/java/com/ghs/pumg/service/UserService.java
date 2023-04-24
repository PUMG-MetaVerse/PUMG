package com.ghs.pumg.service;

import com.ghs.pumg.dto.request.UserHealingCharacterReq;
import com.ghs.pumg.dto.request.UserLoginReq;
import com.ghs.pumg.dto.request.UserSignUpReq;
import com.ghs.pumg.dto.request.UserWorldCharacterReq;
import com.ghs.pumg.dto.response.*;
import com.ghs.pumg.entity.Avatar;
import com.ghs.pumg.entity.User;
import com.ghs.pumg.entity.UserAvatar;
import com.ghs.pumg.repository.AvatarRepository;
import com.ghs.pumg.repository.UserAvatarRepository;
import com.ghs.pumg.repository.UserRepository;

import lombok.RequiredArgsConstructor;

import org.springframework.stereotype.Service;

import javax.transaction.Transactional;
import java.util.List;
import java.util.Optional;


@Service("UserService")
@RequiredArgsConstructor
public class UserService {
    final UserRepository userRepository;
    final AvatarRepository avatarRepository;
    final UserAvatarRepository userAvatarRepository;
    @Transactional
    public UserSignUpRes signUpUser(UserSignUpReq userSignUpReq) {

        User user = User.builder()
                .id(userSignUpReq.getUserId())
                .password(userSignUpReq.getUserPassword())
                .build();
        User userRes = userRepository.save(user);
        return UserSignUpRes.of(200, "Success", userRes);
    }
    @Transactional
    public UserLoginRes loginUser(UserLoginReq userLoginReq) {
        String userId = userLoginReq.getUserId();
        String password = userLoginReq.getUserPassword();

        Optional<User> user = userRepository.findUserById(userId);
        // 로그인 요청한 유저로부터 입력된 패스워드 와 디비에 저장된 유저의 암호화된 패스워드가 같은지 확인.(유효한 패스워드인지 여부 확인)
        if (!user.isPresent()){
            return UserLoginRes.of(201, "해당하는 유저가 존재하지 않습니다.", null, null);
        }
        if (password.equals(user.get().getPassword())) {
            System.out.println(user.get());
            List<UserAvatar> userAvatar = userAvatarRepository.findUserAvatarByUserIdx(user.get());
            System.out.println(userAvatar);
            // 유효한 패스워드가 맞는 경우, 로그인 성공으로 응답.(액세스 토큰을 포함하여 응답값 전달)
            return UserLoginRes.of(200, "Success", user.get(), userAvatar);
        }
        // 유효하지 않는 패스워드인 경우, 로그인 실패로 응답.

        return UserLoginRes.of(200, "Success", null, null);
    }

    // 월드 유저 아바타 세팅 및 닉네임 세팅
    @Transactional
    public UserWorldCharacterRes setWorldCharacter(UserWorldCharacterReq worldInfo) {

        Optional<User> user = userRepository.findById(worldInfo.getUserIdx());
        Optional<Avatar> avatar = avatarRepository.findById(worldInfo.getCharacterIdx());
        UserAvatar userAvatar = UserAvatar.builder()
                .userIdx(user.get())
                .avatarIdx(avatar.get())
                .build();

        UserAvatar userAvatarRes = userAvatarRepository.save(userAvatar);

        User getUser = user.get();
        getUser.modifyNickname(worldInfo.getNickname());
        User userRes = userRepository.save(getUser);
        return UserWorldCharacterRes.of(200, "Success", userRes);
    }
    // 힐링월드 유저 아바타 세팅
    @Transactional
    public UserHealingCharacterRes setHealingCharacter(UserHealingCharacterReq worldInfo) {

        Optional<User> user = userRepository.findById(worldInfo.getUserIdx());
        Optional<Avatar> avatar = avatarRepository.findById(worldInfo.getCharacterIdx());
        UserAvatar userAvatar = UserAvatar.builder()
                .userIdx(user.get())
                .avatarIdx(avatar.get())
                .build();

        UserAvatar userRes = userAvatarRepository.save(userAvatar);

        return UserHealingCharacterRes.of(200, "Success", userRes);
    }
}
