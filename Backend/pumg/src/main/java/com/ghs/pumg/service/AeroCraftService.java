package com.ghs.pumg.service;

import com.ghs.pumg.dto.request.*;
import com.ghs.pumg.dto.response.*;
import com.ghs.pumg.entity.*;
import com.ghs.pumg.repository.AvatarRepository;
import com.ghs.pumg.repository.RankingRepository;
import com.ghs.pumg.repository.UserAvatarRepository;
import com.ghs.pumg.repository.UserRepository;
import lombok.RequiredArgsConstructor;
import lombok.extern.java.Log;
import org.springframework.stereotype.Service;

import javax.transaction.Transactional;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.Function;
import java.util.function.Predicate;
import java.util.stream.Collectors;
import java.util.stream.*;
import java.util.*;
import java.util.concurrent.*;
import java.util.function.*;

@Log
@Service("AeroCraftService")
@RequiredArgsConstructor
public class AeroCraftService {
    final UserRepository userRepository;
    final RankingRepository rankingRepository;
    @Transactional
    public BaseResponseBody recordRanking(AeroCraftRecordRankingReq rankingInfo) {
        Optional<User> user = userRepository.findById(rankingInfo.getUserIdx());

        Ranking rank = Ranking.builder()
                .userIdx(user.get())
                .score(rankingInfo.getScore())
                .clearTime(rankingInfo.getClearTime())
                .build();
        rankingRepository.save(rank);
        return BaseResponseBody.of(200, "Success");
    }
    @Transactional
    public AeroCraftRankingRes getRanking() {
        List<Ranking> res = rankingRepository.findTopRankings();
        System.out.println(res);
        res = res.stream()
                .filter(distinctByKey(Ranking::getUserIdx))
                .collect(Collectors.toList());
        System.out.println(res);
        return AeroCraftRankingRes.of(200, "Success", res);
    }
    @Transactional
    public AeroCraftRankingRes getRaceRanking() {
        List<Ranking> res = rankingRepository.findByScoreOrderByClearTime(-1);
        System.out.println(res);

        // 플레이어 별로 데이터를 그룹화하고, 각 그룹의 개수를 센다
        Map<User, Long> countByUser = res.stream()
                .collect(Collectors.groupingBy(Ranking::getUserIdx, Collectors.counting()));
        System.out.println(countByUser);

        // 기존의 distinct filter 로직
        res = res.stream()
                .filter(distinctByKey(Ranking::getUserIdx))
                .collect(Collectors.toList());
        System.out.println(res);
        List<AeroCraftRankingRes.Response> response = new LinkedList<>();
//         결과에 플레이어별 게임 횟수를 추가
        for (Ranking ranking : res) {
            response.add(new AeroCraftRankingRes.Response(ranking));
            response.get(response.size()-1).setScore(countByUser.get(ranking.getUserIdx()).intValue());
//            ranking.modifyScore(countByUser.get(ranking.getUserIdx()).intValue());
        }

        return AeroCraftRankingRes.ofRace(200, "Success", response);
    }

    private static <T> Predicate<T> distinctByKey(Function<? super T, ?> keyExtractor) {
        Set<Object> seen = ConcurrentHashMap.newKeySet();
        return t -> seen.add(keyExtractor.apply(t));
    }
}
