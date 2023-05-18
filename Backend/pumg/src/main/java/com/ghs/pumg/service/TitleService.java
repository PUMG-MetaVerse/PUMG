package com.ghs.pumg.service;

import com.ghs.pumg.dto.request.AeroCraftRecordRankingReq;
import com.ghs.pumg.dto.request.TitleLockOnReq;
import com.ghs.pumg.dto.request.TitleSetReq;
import com.ghs.pumg.dto.response.AeroCraftRankingRes;
import com.ghs.pumg.dto.response.BaseResponseBody;
import com.ghs.pumg.dto.response.UserTitleListRes;
import com.ghs.pumg.entity.Ranking;
import com.ghs.pumg.entity.Title;
import com.ghs.pumg.entity.User;
import com.ghs.pumg.entity.UserTitle;
import com.ghs.pumg.repository.RankingRepository;
import com.ghs.pumg.repository.TitleRepository;
import com.ghs.pumg.repository.UserRepository;
import com.ghs.pumg.repository.UserTitleRepository;
import lombok.RequiredArgsConstructor;
import lombok.extern.java.Log;
import org.springframework.stereotype.Service;

import javax.transaction.Transactional;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.Function;
import java.util.function.Predicate;
import java.util.stream.Collectors;

@Log
@Service("TitleService")
@RequiredArgsConstructor
public class TitleService {
    final UserRepository userRepository;
    final TitleRepository titleRepository;
    final UserTitleRepository userTitleRepository;
    @Transactional
    public BaseResponseBody recordLockOnGood(TitleLockOnReq titleInfo) {
        Optional<User> user = userRepository.findById(titleInfo.getUserIdx());
        Optional<Title> title = titleRepository.findById(1L);
        Optional<UserTitle> checkHaveTitle = userTitleRepository.findByUserIdxAndTitleIdx(user.get(), title.get());
        if (checkHaveTitle.isEmpty()) {
            UserTitle userTitle = UserTitle.builder()
                    .userIdx(user.get())
                    .titleIdx(title.get())
                    .build();
            userTitleRepository.save(userTitle);
            return BaseResponseBody.of(200, "Success");
        } else {
            return BaseResponseBody.of(202, "칭호 이미 있엉");
        }
    }
    @Transactional
    public BaseResponseBody recordLockOnPerfect(TitleLockOnReq titleInfo) {
        Optional<User> user = userRepository.findById(titleInfo.getUserIdx());
        Optional<Title> title = titleRepository.findById(2L);
        Optional<UserTitle> checkHaveTitle = userTitleRepository.findByUserIdxAndTitleIdx(user.get(), title.get());
        if (checkHaveTitle.isEmpty()) {
            UserTitle userTitle = UserTitle.builder()
                    .userIdx(user.get())
                    .titleIdx(title.get())
                    .build();
            userTitleRepository.save(userTitle);
            return BaseResponseBody.of(200, "Success");
        } else {
            return BaseResponseBody.of(202, "칭호 이미 있엉");
        }
    }
    @Transactional
    public BaseResponseBody recordSkyRush(TitleLockOnReq titleInfo) {
        Optional<User> user = userRepository.findById(titleInfo.getUserIdx());
        Optional<Title> title = titleRepository.findById(3L);
        Optional<UserTitle> checkHaveTitle = userTitleRepository.findByUserIdxAndTitleIdx(user.get(), title.get());
        if (checkHaveTitle.isEmpty()) {
            UserTitle userTitle = UserTitle.builder()
                    .userIdx(user.get())
                    .titleIdx(title.get())
                    .build();
            userTitleRepository.save(userTitle);
            return BaseResponseBody.of(200, "Success");
        } else {
            return BaseResponseBody.of(202, "칭호 이미 있엉");
        }
    }
    @Transactional
    public UserTitleListRes getTitle(Long userIdx) {
        Optional<User> user = userRepository.findById(userIdx);
        List<UserTitle> userTitle = userTitleRepository.findByUserIdx(user.get());
        return UserTitleListRes.of(200, "Success", userTitle);
    }
    @Transactional
    public BaseResponseBody setTitle(TitleSetReq userInfo) {
        Optional<User> user = userRepository.findById(userInfo.getUserIdx());
        Optional<Title> title = titleRepository.findById((userInfo.getTitleIdx()));
        user.get().modifyTitle(title.get().getTitle());
        userRepository.save(user.get());
        return UserTitleListRes.of(200, "Success");
    }
    @Transactional
    public BaseResponseBody earnTitle(TitleSetReq titleInfo) {
        Optional<User> user = userRepository.findById(titleInfo.getUserIdx());
        Optional<Title> title = titleRepository.findById(titleInfo.getTitleIdx());
        Optional<UserTitle> checkHaveTitle = userTitleRepository.findByUserIdxAndTitleIdx(user.get(), title.get());
        if (checkHaveTitle.isEmpty()) {
            UserTitle userTitle = UserTitle.builder()
                    .userIdx(user.get())
                    .titleIdx(title.get())
                    .build();
            userTitleRepository.save(userTitle);
            return BaseResponseBody.of(200, "Success");
        }else {
            return BaseResponseBody.of(500, "Have title");
        }
    }
    @Transactional
    public BaseResponseBody checkTitle(TitleSetReq titleInfo) {
        Optional<User> user = userRepository.findById(titleInfo.getUserIdx());
        Optional<Title> title = titleRepository.findById(titleInfo.getTitleIdx());
        Optional<UserTitle> checkHaveTitle = userTitleRepository.findByUserIdxAndTitleIdx(user.get(), title.get());
        if (checkHaveTitle.isEmpty()) {
            return BaseResponseBody.of(200, "Success");
        }else {
            return BaseResponseBody.of(500, "Have title");
        }
    }
}
