package com.ghs.pumg.dto.response;

import com.ghs.pumg.entity.Ranking;
import com.ghs.pumg.entity.UserAvatar;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.LinkedList;
import java.util.List;

@Getter
@Setter
public class AeroCraftRankingRes extends DataResponseBody {
    @Getter
    @Setter
    @NoArgsConstructor
    public static class Response {
        private Long idx;
        private Long userIdx;
        private String nickName;

        private int score;
        private String clearTime;

        public Response(Ranking entity){
            this.idx = entity.getIdx();
            this.userIdx = entity.getUserIdx().getIdx();
            this.nickName = entity.getUserIdx().getNickName();
            this.score = entity.getScore();
            this.clearTime = entity.getClearTime();
        }
    }
    public static AeroCraftRankingRes ofRace(Integer statusCode, String message, List<Response> rankingInfo) {
        AeroCraftRankingRes res = new AeroCraftRankingRes();
//        List<Response> response = new LinkedList<>();
//        for (int i = 0; i < rankingInfo.size(); i++) {
//            response.add(new Response(rankingInfo.get(i)));
//        }
        res.setStatus(statusCode);
        res.setMessage(message);
        res.getData().put("rankingInfo", rankingInfo);
//        res.getData().put("access-token", accessToken);
//        res.getData().put("sellerIdx", idx);
        return res;
    }
    public static AeroCraftRankingRes of(Integer statusCode, String message, List<Ranking> rankingInfo) {
        AeroCraftRankingRes res = new AeroCraftRankingRes();
        List<Response> response = new LinkedList<>();
        for (int i = 0; i < rankingInfo.size(); i++) {
            response.add(new Response(rankingInfo.get(i)));
        }
        res.setStatus(statusCode);
        res.setMessage(message);
        res.getData().put("rankingInfo", response);
        return res;
    }
}
