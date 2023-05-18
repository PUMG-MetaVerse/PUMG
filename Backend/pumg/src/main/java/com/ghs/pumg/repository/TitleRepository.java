package com.ghs.pumg.repository;

import com.ghs.pumg.entity.Ranking;
import com.ghs.pumg.entity.Title;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface TitleRepository extends JpaRepository<Title, Long> {


//    @Query("SELECT r FROM Ranking r WHERE r.userIdx IN (SELECT DISTINCT rr.userIdx FROM Ranking rr) ORDER BY r.score DESC, r.clearTime ASC")
//    @Query("SELECT r FROM Ranking r WHERE r.score >= 0 ORDER BY r.score DESC, r.clearTime ASC")
//    List<Ranking> findTopRankings();
//
////    List<Ranking> findAllBy(int score);
//    List<Ranking> findByScoreOrderByClearTime(int score);
}
